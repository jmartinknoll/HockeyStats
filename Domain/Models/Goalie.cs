using Domain.Models.HockeyStatsApi;

namespace Domain.Models
{
    public class Goalie
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string TeamCode { get; set; }
        public string Position { get; set; }

        // Projected playoff stats
        public int ProjectedWins { get; set; }
        public int ProjectedShutouts { get; set; }

        // Optional: store regular season stats for reference
        public int? GamesPlayed { get; set; }
        public int? Wins { get; set; }
        public int? Shutouts { get; set; }

        // Optional: store the source NHLGoalie if needed
        public NHLGoalie? SourceStats { get; set; }
    }
}
