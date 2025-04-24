using Domain.Models.HockeyStatsApi;

namespace Domain.Models
{
    public class Player
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string TeamCode { get; set; }
        public string Position { get; set; }

        // Projected playoff stats
        public int ProjectedGoals { get; set; }
        public int ProjectedAssists { get; set; }

        // Optional: store regular season stats for reference
        public int? GamesPlayed { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Points { get; set; }

        // Optional: store the source NHLSkater if needed
        public NHLSkater? SourceStats { get; set; }
    }
}
