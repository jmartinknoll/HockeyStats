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
        public double? PenaltyMinutesPerGame { get; set; }

        public int TotalGamesPlayed { get; set; } = 0;
        public int TotalWins { get; set; } = 0;
        public int TotalLosses { get; set; } = 0;

        public int TotalGoalsFor { get; set; } = 0;
        public int TotalGoalsAgainst { get; set; } = 0;

        // Optional: to track per round
        //public List<RoundStats> RoundStats { get; set; } = new();
        public Dictionary<int, RoundStats> RoundStats { get; set; } = new();

        public Team Clone()
        {
            return new Team
            {
                Name = this.Name,
                Code = this.Code,
                Division = this.Division,
                Conference = this.Conference,
                Seed = this.Seed,
                PenaltyMinutesPerGame = this.PenaltyMinutesPerGame,

                // Reset playoff simulation stats
                TotalGamesPlayed = 0,
                TotalWins = 0,
                TotalLosses = 0,
                TotalGoalsFor = 0,
                TotalGoalsAgainst = 0,

                // Reset RoundStats as an empty dictionary
                RoundStats = new Dictionary<int, RoundStats>()
            };
        }
    }
}
