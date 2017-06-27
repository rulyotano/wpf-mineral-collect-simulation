using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class ProactiveAgent: AgentBase
    {
        private LinkedList<Point> path;
        //private Storage storage;


        public ProactiveAgent(int teamNumber, Storage storage)
            : base(teamNumber, storage)
        {
            path = new LinkedList<Point>();
        }
        
        public override void Move(Map map)
        {
            SetDirection();
            if (!IsCarrying)
            {
                //moverme por las casillas adyacentes para ver si existe algun mineral
                List<Mineral> auxMineral = new List<Mineral>();

                //busca los minerales en las 8 posiciones
                foreach (var mineral in GetAdyacentPoints(map).Union(GetDiagonalPoints(map)).Where(p => map[p.X, p.Y] is Mineral).Select(p => map[p.X, p.Y] as Mineral))
                    auxMineral.Add(mineral);


                //si hay al menos un mineral
                if (auxMineral.Count > 0)
                {
                    //ahora tengo que escojer un mineral de forma aleaoria
                    Random rd = new Random();
                    int index = rd.Next(0, auxMineral.Count);
                    Mineral mineral = auxMineral[index];
                    map.MineralsCounter--;
                    //borrar el mineral del mapa
                    map.RemoveAt(mineral.Position.X, mineral.Position.Y);
                    IsCarrying = true;
                    this.path = MinPath(map, Position, (p) => p.X == Storage.Position.X && p.Y == Storage.Position.Y);
                    return;
                }

                //si llego aqui es porque no encontre minerales adyacentes
                //caso en el que estoy siguiendo un camino
                if (path.Count > 0)
                {
                    Point auxPoint = path.First.Value;
                    //path.RemoveFirst();

                    if (!ExistObstacleInPath(map))     //is no hay un obstaculo donde debo moverme y el mineral esta todavia
                        MoveToPosition(map, Position, auxPoint);
                    //si no espera a que se libere el obstaculo
                }
                else
                {
                    //caso que no tengo ningun camino calculado
                    path = MinPath(map, Position, (p) => map[p.X, p.Y] is Mineral);
                    SetDirection();

                    if (path.Count > 0)
                        MoveToPosition(map, Position, path.First.Value);
                }
            }
            //caso en el que estoy cargando mineral
            else 
            {
                //llegue al almacen
                if (path.Count == 1)
                {
                    Storage.CollectedMinerals++;
                    IsCarrying = false;
                    path.Clear();
                }
                else if (path.Count > 1)        //estoy en camino
                {
                    Point auxPoint = path.First.Value;

                    if (!ExistObstacleInPath(map))
                        MoveToPosition(map, Position, auxPoint);
                }
                else
                {
                    //caso que no tengo ningun camino calculado
                    path = MinPath(map, Position, (p) => p.X == Storage.Position.X && p.Y == Storage.Position.Y);
                    SetDirection();
                    //ahora tengo que moverme

                    if (path.Count > 0)
                        MoveToPosition(map, Position, path.First.Value);
                }
            }
        }

        private void MoveToPosition(Map map , Point origin, Point dest)
        {
//            map[origin.X, origin.Y] = null;
//            map[dest.X, dest.Y] = this;
            if (path.Count > 1)
                map.Move(origin.X, origin.Y, dest.X, dest.Y);
            path.RemoveFirst();
        }


        private bool ExistObstacleInPath(Map map)
        {
            if (path.Count > 0)
            {
                Point auxPoint = path.First.Value;
                return map[auxPoint.X, auxPoint.Y] != null;
            }
            return false;
        }

        private LinkedList<Point> MinPath(Map map, Point point, Func<Point, bool> condition)
        {
            int[,] auxMap = new int[map.Size, map.Size];
            auxMap[point.X, point.Y] = 1;

            Record record = new Record(null, point);
            Queue<Record> q = new Queue<Record>();
            q.Enqueue(record);

            while (q.Count > 0)
            {
                Record aux = q.Dequeue();

                foreach (var adyacentPoint in GetAdyacentPoints(aux.Point, map))
                {
                    var recAux = new Record(aux, adyacentPoint);

                    //si se cumple la condicion que estamos buscando
                    if (condition(recAux.Point))
                        return BuildPath(recAux);

                    if (map[adyacentPoint.X, adyacentPoint.Y] == null && auxMap[adyacentPoint.X, adyacentPoint.Y] == 0)
                    {
                        auxMap[adyacentPoint.X, adyacentPoint.Y] = auxMap[aux.Point.X, aux.Point.Y] + 1;
                        q.Enqueue(recAux);
                    }
                }

                //busca en las diagonales
                foreach (var diagonalPoint in GetDiagonalPoints(aux.Point, map))
                {
                    var recAux = new Record(aux, diagonalPoint);
                    //si se cumple la condicion que estamos buscando
                    if (condition(recAux.Point))
                        return BuildPath(recAux);

                }
            }

            //si llego aqui es que no se puedo mover para ningun lado
            return new LinkedList<Point>();
        }

        private LinkedList<Point> BuildPath(Record root) {
            LinkedList<Point> path = new LinkedList<Point>();
            path.AddFirst(root.Point);

            while (root.Parent != null)
            {
                path.AddFirst(root.Parent.Point);
                root = root.Parent;
            }
            path.RemoveFirst();      //quit my position

            return path;
        }

        private void SetDirection()
        {
            if (path.Count > 0)
            {
                SetDirection(path.First.Value);
            }
        }
        
        private class Record 
        {
            public Record(Record parent, Point point) {
                Parent = parent;
                Point = point;
            }

            public Record Parent { get; set; }
            public Point Point { get; set; }
        }
    }
}
