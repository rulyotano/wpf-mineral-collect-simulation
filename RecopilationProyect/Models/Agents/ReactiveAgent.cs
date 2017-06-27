using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class ReactiveAgent: AgentBase
    {
        public ReactiveAgent(int teamNumber, Storage storage) : base(teamNumber, storage)
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
                    return;
                }
            }

            //Goto base or Move Random
            if (IsCarrying)
            {
                var freePoints = GetAdyacentPoints(map).Where(p => map[p.X, p.Y] == null);
                var point = NearestPoint(Storage.Position, freePoints);
                if (point != null)
                {
                    SetDirection(point);
                    map.Move(Position.X, Position.Y, point.X, point.Y);
                }
            }
            else
            {

                var point = GetAdyacentFreePointRandom(map);
                if (point != null)
                {
                    SetDirection(point);
                    map.Move(Position.X, Position.Y, point.X, point.Y);
                }
            }
        }

        private Point NearestPoint(Point p, IEnumerable<Point> points)
        {
            double minDistance = double.MaxValue;
            Point minPoint = null;

            foreach (var point in points)
            {
                double tDistance = Math.Abs(Math.Sqrt(Math.Pow(p.X - point.X, 2) + Math.Pow(p.Y - point.Y, 2)));
                if (tDistance < minDistance)
                {
                    minDistance = tDistance;
                    minPoint = point;
                }
            }

            return minPoint;
        }
    }

}
