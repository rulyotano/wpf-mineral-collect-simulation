using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using RecopilationProyect.Models;

namespace RecopilationProyect
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
//            SimulationManager sm = new SimulationManager(5, 30, 3, 4);
//            StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "file.txt"));
//            for (int i = 0; i < sm.Turns; i++)
//            {
//                sw.WriteLine("Turn : {0}", i + 1);
//                sm.Simulate();
//            }
//
//            int winnerTeam = 0;
//            int max = int.MinValue;
//
//            foreach (var teamManager in sm.TeamManagers)
//            {
//                if (teamManager.Storage.CollectedMinerals > max)
//                    winnerTeam = teamManager.NumberOfTeam;
//            }
//
//            sw.WriteLine(String.Format("The winner team is {0}", winnerTeam));
//            sw.WriteLine("Ended");
//            sw.Close();
        }
    }
}
