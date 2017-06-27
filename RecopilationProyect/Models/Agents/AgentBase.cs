using System;
using System.Collections.Generic;
using System.Linq;
using RecopilationProyect.Utils;

namespace RecopilationProyect.Models
{
    public abstract class AgentBase : EnvironmentObject
    {
        protected int[] dx = { -1, 0, 0, 1, 1, -1, 1, -1 };
        protected int[] dy = { 0, 1, -1, 0, 1, -1, -1, 1 };
        protected Storage Storage { get; set; }


        public AgentBase(int teamNumber, Storage storage)
        {
            TeamNumber = teamNumber;
            Storage = storage;
        }

        #region IsCarrying
        private bool _isCarrying;
        public bool IsCarrying
        {
            get { return _isCarrying; }
            set
            {
                _isCarrying = value;
                RaisePropertyChanged("IsCarrying");
            }
        }

        #endregion

        #region TeamNumber
        private int _teamNumber;
        public int TeamNumber
        {
            get { return _teamNumber; }
            set
            {
                _teamNumber = value;
                RaisePropertyChanged("TeamNumber");
            }
        }

        #endregion

        #region Direction

        private int _direction = 0;
        /// <summary>
        /// 0 - Down, 1 - Left, 2 - Up, 3 - Right
        /// </summary>
        public int Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                RaisePropertyChanged("Direction");
            }
        }

        #endregion

        public abstract void Move(Map map);

        protected void SetDirection(Point nextPoint)
        {
            int dirX = nextPoint.X - Position.X;
            int dirY = nextPoint.Y - Position.Y;
            if (dirX == 1 && dirY == 0)             //down
                Direction = 0;
            else if (dirX == 0 && dirY == -1)        //left
                Direction = 1;
            else if (dirX == -1 && dirY == 0)        //up
                Direction = 2;
            else if (dirX == 0 && dirY == 1)        //right
                Direction = 3;
        }

        protected IEnumerable<Point> GetAdyacentPoints(Map map)
        {
            return GetAdyacentPoints(Position, map);
        }

        protected IEnumerable<Point> GetDiagonalPoints(Map map)
        {
            return GetDiagonalPoints(Position, map);
        }

        protected IEnumerable<Point> GetAdyacentPoints(Point point, Map map)
        {
            for (int i = 0; i < 4; i++)
            {
                int x = point.X + dx[i];
                int y = point.Y + dy[i];

                if (x >= 0 && x < map.Size && y >= 0 && y < map.Size)
                {
                    yield return new Point(x, y);
                }
            }
        }

        protected IEnumerable<Point> GetDiagonalPoints(Point point,Map map)
        {
            var toRet = new List<Point>();
            for (int i = 4; i < 8; i++)
            {
                int x = point.X + dx[i];
                int y = point.Y + dy[i];

                if (x >= 0 && x < map.Size && y >= 0 && y < map.Size)
                {
                    yield return new Point(x, y);
                }
            }
        }

        protected Point GetAdyacentFreePointRandom(Map map)
        {
            var freePoints = GetAdyacentPoints(map).Where(p => map[p.X, p.Y] == null).ToList();
            var r = new Random();
            if (freePoints.Count > 0)
            {
                ThreadUtils.Sleep(10);       //Sleep 10 mlsecond for random
                return freePoints[r.Next(0, freePoints.Count)];
            }
            return null;
        }

    }
}