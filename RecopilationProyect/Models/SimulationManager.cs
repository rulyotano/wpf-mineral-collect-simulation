using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using RecopilationProyect.Utils;

namespace RecopilationProyect.Models
{
    public class SimulationManager: INotifyPropertyChanged
    {
        
        public SimulationManager(int turns, int size, int numberOfAgents, IList<Type> agentTypes)
        {
            int numberOfTeams = agentTypes.Count();
            while (numberOfTeams * numberOfAgents * 2 >= size * size)
                numberOfAgents--;

            if (numberOfAgents == 0) throw new Exception("The simulation can't start because a configuration problem with the number of teams");

            this.Turns = turns;
            
            _teamManagers = new TeamManager[numberOfTeams];
            Map = new Map(size);

            var i = 0;
            foreach (var agentType in agentTypes)
            {
                _teamManagers[i] = Generator.GenerateTeam(Map, numberOfAgents, i, agentType);
                _teamManagers[i].Type = agentType;
                i++;
            }

            Generator.GenerateMinerals(Map);
        }

        public void Simulate()
        {
            if (CurrentTurns > Turns)
                return;

            //falta Generar minerales aleatorios por el mapa
            CurrentTurns++;

            List<AgentBase> auxAgents = new List<AgentBase>();

            for (int i = 0; i < _teamManagers.Length; i++)
                for (int j = 0; j < _teamManagers[i].Agents.Count; j++)
                    auxAgents.Add(_teamManagers[i].Agents[j]);

            Random rd = new Random();
            while (auxAgents.Count > 0)
            {
                int index = rd.Next(0, auxAgents.Count);

                auxAgents[index].Move(Map);
                //ThreadUtils.Sleep(10);       //Sleep 10 mlsecond for random
                auxAgents.RemoveAt(index);
            }

            Generator.GenerateMineralsPerTurn(Map);
        }

        #region Properties

        private int _turns;
        public int Turns
        {
            get { return _turns; }
            set
            {
                _turns = value;
                RaisePropertyChanged("Turns");
            }
        }

        private int _currentTurns;
        public int CurrentTurns
        {
            get { return _currentTurns; }
            set
            {
                _currentTurns = value;
                RaisePropertyChanged("CurrentTurns");
            }
        }

        public Map Map { get; private set; }

        #region TeamManagers

        private readonly TeamManager[] _teamManagers;

        public IEnumerable<TeamManager> TeamManagerManagers
        {
            get { return _teamManagers; }
        }

        #endregion


        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
