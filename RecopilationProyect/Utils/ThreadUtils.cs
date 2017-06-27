using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace RecopilationProyect.Utils
{
    public class ThreadUtils
    {
        public static void Sleep(int milisecond)
        {
            using (Dispatcher.CurrentDispatcher.DisableProcessing())
            {
                Thread.Sleep(milisecond);
            }
        }
    }
}
