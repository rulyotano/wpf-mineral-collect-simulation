using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class Storage : EnvironmentObject
    {
        public int TeamNumber { get; set; }

        private int _collectedMinerals;
        public int CollectedMinerals
        {
            get { return _collectedMinerals; }
            set { _collectedMinerals = value;
            RaisePropertyChanged("CollectedMinerals");
            }
        }

        public override string ToString()
        {
            return string.Format("Team: {0}, minerals: {1}", TeamNumber, CollectedMinerals);
        }
    }
}
