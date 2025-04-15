using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Team
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public int Seed { get; set; }

        public int TotalGamesPlayed { get; set; } = 0;
        public int TotalGoalsFor { get; set; } = 0;
        public int TotalGoalsAgainst { get; set; } = 0;

        // Optional: to track per round
        //public List<RoundStats> RoundStats { get; set; } = new();
        public Dictionary<int, RoundStats> RoundStats { get; set; } = new();
    }
}
