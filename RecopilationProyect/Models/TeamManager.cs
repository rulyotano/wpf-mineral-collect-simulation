using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecopilationProyect.Models
{
    public class TeamManager
    {
        public TeamManager(int numberOfTeam) {
            NumberOfTeam = numberOfTeam;
            Agents = new List<AgentBase>();
        }

        public Storage Storage { get; set; }
        public int NumberOfTeam { get; set; }
        public List<AgentBase> Agents { get; set; }
        public Type Type { get; set; }

    }
}
