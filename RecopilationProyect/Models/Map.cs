using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class Map
    {
        EnvironmentObject[,] env;
        
        //int teams;
        //int numberOfAgents;
        int n;
        int mineralsCounter = 0;

        public Map(int n/* , int teams , int numberOfAgents*/) {
            //this.teams = teams;
            //this.numberOfAgents = numberOfAgents;
            this.n = n;
            this.env = new EnvironmentObject[n, n];
            FreeCells = new List<Point>();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    FreeCells.Add(new Point(i, j));
        }

        public List<Point> FreeCells { get; set; }

        public int Size { get { return n; } }

        public int MineralsCounter 
        {
            get { return mineralsCounter; }
            set { mineralsCounter = value; }
        }

        public EnvironmentObject this[int x, int y]
        {
            get { return env[x, y]; }

            //here im going to update the AllItems list
            private set
            {
               env[x, y] = value;
            }
        }

        private void DeletePointFromFreeCells(int x, int y) 
        {
            for (int i = 0; i < FreeCells.Count; i++)
                if(FreeCells[i].X == x && FreeCells[i].Y ==  y) {
                    FreeCells.RemoveAt(i);
                    return;
                }
        }

        private void AddPointToFreeCells(int x, int y) 
        {
            FreeCells.Add(new Point(x, y));
        }

        public void AddAt(EnvironmentObject item, int x, int y , int index = -1)
        {
            this[x, y] = item;
            if (index == -1)
                DeletePointFromFreeCells(x, y);
            else
                FreeCells.RemoveAt(index);
            
            AllItems.Add(item);
        }

        public void RemoveAt(int x, int y , int index = -1)
        {
            var item = this[x, y];
            this[x, y] = null;
            if (index == -1)
                AddPointToFreeCells(x, y);
            else
                FreeCells.RemoveAt(index);
            
            AllItems.Remove(item);
        }

        public void Move(int fromX, int fromY, int toX, int toY)
        {
            var item = this[fromX, fromY];
            this[fromX, fromY] = null;
            this[toX, toY] = item;
            item.Position = new Point(toX, toY);
            AddPointToFreeCells(fromX, fromY);
            DeletePointFromFreeCells(toX, toY);
        }

        public bool CanMoveTo(int x, int y)
        {
            return this[x, y] == null;
        }

        private ObservableCollection<EnvironmentObject> _allItems;
        public ObservableCollection<EnvironmentObject> AllItems
        {
            get { return _allItems ?? (_allItems = new ObservableCollection<EnvironmentObject>()); }
        }
    }
}
