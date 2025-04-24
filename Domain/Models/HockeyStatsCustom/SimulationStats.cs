namespace Domain.Models.HockeyStatsCustom
{
    public class SimulationStats
    {
        public string TeamCode { get; set; }
        public int Simulations { get; set; } = 0;

        // Accumulated stats
        public int TotalGamesPlayed { get; set; } = 0;
        public int TotalWins { get; set; } = 0;
        public int TotalLosses { get; set; } = 0;
        public int TotalGoalsFor { get; set; } = 0;
        public int TotalGoalsAgainst { get; set; } = 0;

        // Average stats
        public double AvgGamesPlayed { get; set; } = 0;
        public double AvgWins { get; set; } = 0;
        public double AvgLosses { get; set; } = 0;
        public double AvgGoalsFor { get; set; } = 0;
        public double AvgGoalsAgainst { get; set; } = 0;
    }
}
