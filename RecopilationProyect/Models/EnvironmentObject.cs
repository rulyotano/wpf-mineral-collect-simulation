using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class EnvironmentObject: INotifyPropertyChanged
    {
        private Point _position;
        public Point Position
        {
            get { return _position; }
            set { _position = value;
            RaisePropertyChanged("Position");
            }
        }

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
