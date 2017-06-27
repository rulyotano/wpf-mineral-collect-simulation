using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using RecopilationProyect.Common;

namespace RecopilationProyect.Implementations
{
    public class Message: IMessage
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
