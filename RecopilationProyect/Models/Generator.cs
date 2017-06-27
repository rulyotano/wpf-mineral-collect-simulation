using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecopilationProyect.Utils;

namespace RecopilationProyect.Models
{
    public static class Generator
    {
        public static TeamManager GenerateTeam(Map map, int nAgents, int numberOfTeam, Type agentsType)
        {
            var tm = new TeamManager(numberOfTeam);

            Random rd = new Random();

            int count = 0;
            int index = 0;
            Point aux;
            
            ThreadUtils.Sleep(8);       //for random
            index = rd.Next(0, map.FreeCells.Count);

            aux = map.FreeCells[index];
            Storage storage = new Storage { Position = aux , TeamNumber = numberOfTeam };
            map.AddAt(storage,aux.X, aux.Y , index);

            while (count < nAgents)
            {
                ThreadUtils.Sleep(8);       //for random
                index = rd.Next(0, map.FreeCells.Count);

                aux = map.FreeCells[index];

                AgentBase ag = GetNewAgent(agentsType, numberOfTeam, storage);
                ag.Position = aux;
                map.AddAt(ag, aux.X, aux.Y , index);
                tm.Agents.Add(ag);
                count++;
            }

            tm.Storage = storage;
            return tm;
        }

        public static void GenerateMinerals(Map map)
        {
            //30% de minerales por mapa
            int nminerals = (3 * map.Size * map.Size) / 10;

            int index = 0;
            Random rd = new Random();
            Point aux;

            for (int i = 0; i < nminerals; i++)
            {
                index = rd.Next(0, map.FreeCells.Count);
                aux = map.FreeCells[index];
                Mineral mineral = new Mineral() { Position = aux };
                map.AddAt(mineral, aux.X, aux.Y, index);
                map.MineralsCounter++;
            }
        }

        public static void GenerateMineralsPerTurn(Map map) 
        {
            int index = -1;
            double prob = 0;
            int difference = ((3 * map.Size * map.Size) / 10) - map.MineralsCounter;
            Random rd = new Random();
            Point aux;

            for (int i = 0; i < difference; i++)
            {
                prob = rd.NextDouble();
                if (prob >= 0.90) 
                {
                    index = rd.Next(0, map.FreeCells.Count);
                    aux = map.FreeCells[index];
                    Mineral mineral = new Mineral() { Position = aux };
                    map.AddAt(mineral, aux.X, aux.Y, index);
                    map.MineralsCounter++;
                }
            }
        }

        public static AgentBase GetNewAgent(Type agentType, int teamNumber, Storage s)
        {
            try
            {
                return (AgentBase)agentType.GetConstructor(new Type[] {typeof (int), typeof (Storage)}).Invoke(new object[] {teamNumber, s});
            }
            catch (Exception e)
            {
                throw new InvalidCastException(
                    string.Format("Can't create an instance of type {0} with parameters: {1}, {2}", agentType,
                                  teamNumber, s));

            }
        }
    }
}
