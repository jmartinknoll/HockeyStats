using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.HockeyStatsApi
{
    public class NHLGoalie
    {
        public int? PlayerId { get; set; }
        public string? Headshot { get; set; }
        public NHLName? FirstName { get; set; }
        public NHLName? LastName { get; set; }
        public string? PositionCode { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public int? OtLosses { get; set; }
        public double? SavePctg { get; set; }
        public double? GoalsAgainstAverage { get; set; }
        public int? Shutouts { get; set; }
        public int? GoalsAgainst { get; set; }
        public int? ShotsAgainst { get; set; }
        public int? Saves { get; set; }
        public int? PenaltyMinutes { get; set; }
        public double? TimeOnIce { get; set; }
    }
}
