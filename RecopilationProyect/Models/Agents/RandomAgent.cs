using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecopilationProyect.Utils;

namespace RecopilationProyect.Models
{
    public class RandomAgent: AgentBase
    {
        public RandomAgent(int teamNumber, Storage storage) : base(teamNumber, storage)
        {
        }

        public override void Move(Map map)
        {
            if (!IsCarrying)
            {
                //moverme por las casillas adyacentes para ver si existe algun mineral
                //busca los minerales en las 8 posiciones

                List<Mineral> auxMineral = GetAdyacentPoints(map).Union(GetDiagonalPoints(map)).Where(p => map[p.X, p.Y] is Mineral).Select(p => map[p.X, p.Y] as Mineral).ToList();

                //si hay al menos un mineral
                if (auxMineral.Count > 0)
                {
                    //ahora tengo que escojer un mineral de forma aleaoria
                    Random rd = new Random();
                    int index = rd.Next(0, auxMineral.Count - 1);
                    Mineral mineral = auxMineral[index];
                    map.MineralsCounter--;
                    //borrar el mineral del mapa
                    map.RemoveAt(mineral.Position.X, mineral.Position.Y);
                    IsCarrying = true;
                    return;     //porque ya lo cogi
                }
            }
            //caso en el que estoy cargando mineral
            else
            {
                Storage storage = GetAdyacentPoints(map).Union(GetDiagonalPoints(map)).Where(p => map[p.X, p.Y] == Storage).Select(p => map[p.X, p.Y] as Storage).FirstOrDefault();
             
                //llegue al almacen
                if (storage != null)
                {
                    storage.CollectedMinerals++;
                    IsCarrying = false;
                }
            }
            //Move Random
            
            var point = GetAdyacentFreePointRandom(map);
            if (point != null)
            {
                SetDirection(point);
                map.Move(Position.X, Position.Y, point.X, point.Y);
            }
        }
    }
}
