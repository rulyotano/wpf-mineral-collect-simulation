using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using RecopilationProyect.Common;
using RecopilationProyect.Implementations;
using RecopilationProyect.Models;

namespace RecopilationProyect.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region Private Fields
        private DispatcherTimer _timer;
        private IMessage _message = new Message();
        #endregion

        #region Properties

        #region AgentsTypes

        private IEnumerable<Type> _agentsTypes;
        public IEnumerable<Type> AgentsTypes
        {
            get { return _agentsTypes ?? new List<Type>(){typeof(Agent), typeof(RandomAgent), typeof(ReactiveAgent), typeof(ProactiveAgent)}; }
        }

        #endregion

        #region TeamsInfo

        private ObservableCollection<TeamInfoViewModel> _teamsInfo;
        public ObservableCollection<TeamInfoViewModel> TeamsInfo
        {
            get
            {
                if (_teamsInfo == null)
                {
                    _teamsInfo = new ObservableCollection<TeamInfoViewModel>();
                    for (var i = 0; i < Teams; i++)
                    {
                        _teamsInfo.Add(new TeamInfoViewModel() {Name = i.ToString(), Type = AgentsTypes.First()});
                    }
                }
                return _teamsInfo;
            }
        }

        #endregion

        #region Size

        private int _size = 15;
        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                RaisePropertyChanged("Size");
            }
        }

        #endregion

        #region Turns

        private int _turns = 150;
        public int Turns
        {
            get { return _turns; }
            set
            {
                _turns = value;
                RaisePropertyChanged("Turns");
            }
        }

        #endregion

        #region Teams

        private int _teams = 4;
        public int Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                RaisePropertyChanged("Teams");
            }
        }

        #endregion

        #region AgentsPerTeam

        private int _agentsPerTeam = 3;
        public int AgentsPerTeam
        {
            get { return _agentsPerTeam; }
            set
            {
                _agentsPerTeam = value;
                RaisePropertyChanged("AgentsPerTeam");
            }
        }

        #endregion

        #region StepDelay
        private int _stepDelay = 1000;
        public int StepDelay
        {
            get { return _stepDelay; }
            set
            {
                _stepDelay = value;
                RaisePropertyChanged("StepDelay");
            }
        }

        #endregion

        #region IsSimulating

        private bool _isSimulating;
        public bool IsSimulating
        {
            get { return _isSimulating; }
            set
            {
                _isSimulating = value;
                RaisePropertyChanged("IsSimulating");
            }
        }

        #endregion

        #region Mode

        //0 - step by step, 1 - automatic
        private int _mode = 0;
        public int Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                RaisePropertyChanged("Mode");
            }
        }

        #endregion

        #region SimulationManager

        private SimulationManager _simulationManager;
        public SimulationManager SimulationManager
        {
            get { return _simulationManager; }
            set
            {
                _simulationManager = value;
                RaisePropertyChanged("SimulationManager");
            }
        }

        #endregion

        #endregion

        #region Commands

        #region StartSimulationCommand

        private DelegateCommand _startSimulationCommand = null;

        public DelegateCommand StartSimulationCommand
        {
            get
            {
                return _startSimulationCommand ??
                       (_startSimulationCommand = new DelegateCommand(StartSimulation, StartSimulationCanExecute));
            }
            set { _startSimulationCommand = value; }
        }

        private void StartSimulation()
        {
            var types = TeamsInfo.Select(inf => inf.Type).ToArray();
            if (Mode == 1)
            {
                SimulationManager = new SimulationManager(Turns, Size, AgentsPerTeam, types);    
            }
            else if (Mode == 0)
            {
                SimulationManager = new SimulationManager(Turns, Size, AgentsPerTeam, types);       
                _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(StepDelay)};
                _timer.Tick += (_, __) => NextStepCommand.Execute();
                _timer.Start();
            }
            IsSimulating = true;
        }

        private bool StartSimulationCanExecute()
        {
            return !IsSimulating;
        }

        #endregion

        #region StopSimulationCommand

        private DelegateCommand _stopSimulationCommand = null;
        public DelegateCommand StopSimulationCommand
        {
            get
            {
                return _stopSimulationCommand ??
                       (_stopSimulationCommand = new DelegateCommand(StopSimulation, StopSimulationCanExcecute));
            }
            set { _stopSimulationCommand = value; }
        }

        private void StopSimulation()
        {
            if (Mode == 1)
                IsSimulating = false;
            else if (Mode == 0)
            {
                IsSimulating = false;
                _timer.Stop();
                //stop the timer
            }
            _message.ShowMessage(BuildMessage());
        }

        private bool StopSimulationCanExcecute()
        {
            return IsSimulating;
        }

        #endregion

        #region NextStepCommand

        private DelegateCommand _nextStepCommand;
        public DelegateCommand NextStepCommand
        {
            get { return _nextStepCommand ?? (_nextStepCommand = new DelegateCommand(MoveNextStep, NextStepCanExcecute)); }
            set { _nextStepCommand = value; }
        }

        private void MoveNextStep()
        {
            SimulationManager.Simulate();

            NextStepCommand.RaiseCanExecuteChanged();
            if (!NextStepCanExcecute())
                StopSimulation();
        }

        private bool NextStepCanExcecute()
        {
            return IsSimulating && SimulationManager.CurrentTurns < SimulationManager.Turns;
        }
        #endregion

        private void RaiseAllCommandsCanExecuteChanged()
        {
            StartSimulationCommand.RaiseCanExecuteChanged();
            StopSimulationCommand.RaiseCanExecuteChanged();
            NextStepCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region CallBacks

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Teams")
            {
                if (Teams > TeamsInfo.Count)
                {

                    int countItemToAdd = Teams - TeamsInfo.Count;
                    while (countItemToAdd > 0)      //mientras halla que anyadir alguno
                    {
                        TeamsInfo.Add(new TeamInfoViewModel() {Type = AgentsTypes.First(), Name = TeamsInfo.Count.ToString()});
                        countItemToAdd--;
                    }
                }
                else if (Teams < TeamsInfo.Count)
                {
                    int countItemsToRemove = TeamsInfo.Count - Teams;
                    while (countItemsToRemove > 0)      //mientras halla que borrar alguno
                    {
                        TeamsInfo.RemoveAt(TeamsInfo.Count - 1);    //borro el ultimo
                        countItemsToRemove--;
                    }
                }
            }
            RaiseAllCommandsCanExecuteChanged();
        }

        #endregion

        private string BuildMessage()
        {

            TeamManager winnerTeam = null;
            int max = int.MinValue;
            StringBuilder sb = new StringBuilder();

            foreach (var teamManager in SimulationManager.TeamManagerManagers)
            {
                if (teamManager.Storage.CollectedMinerals > max)
                {
                    winnerTeam = teamManager;
                    max = teamManager.Storage.CollectedMinerals;
                }
            }

            int teamWinnersCount =
                SimulationManager.TeamManagerManagers.Count(
                    t => t.Storage.CollectedMinerals == winnerTeam.Storage.CollectedMinerals);

            if (teamWinnersCount == 1)
                sb.AppendFormat("The team winner is {0}!!!\n", winnerTeam.NumberOfTeam);
            else if (teamWinnersCount > 1)
            {
                sb.AppendFormat("There are {0} teams that draws!!!!\n", teamWinnersCount);
            }
            int index = 1;
            foreach (var team in SimulationManager.TeamManagerManagers.OrderByDescending(t => t.Storage.CollectedMinerals))
            {
                sb.AppendFormat("{2}.Team {0} collected {1} minerals.\n", team.NumberOfTeam, team.Storage.CollectedMinerals, index++);
            }
            return sb.ToString();
        }
    }
}