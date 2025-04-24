namespace Domain.Models.HockeyStatsApi
{
    public class NHLTeamStats
    {
        public double? FaceoffWinPct { get; set; }
        public int? GamesPlayed { get; set; }
        public int? GoalsAgainst { get; set; }
        public double? GoalsAgainstPerGame { get; set; }
        public int? GoalsFor { get; set; }
        public double? GoalsForPerGame { get; set; }
        public int? Losses { get; set; }
        public int? OtLosses { get; set; }
        public double? PenaltyKillNetPct { get; set; }
        public double? PenaltyKillPct { get; set; }
        public double? PointPct { get; set; }
        public int? Points { get; set; }
        public double? PowerPlayNetPct { get; set; }
        public double? PowerPlayPct { get; set; }
        public int? RegulationAndOtWins { get; set; }
        public string? SeasonId { get; set; }
        public double? ShotsAgainstPerGame { get; set; }
        public double? ShotsForPerGame { get; set; }
        public string? TeamFullName { get; set; } = string.Empty;
        public int? TeamId { get; set; }
        public int? Ties { get; set; }
        public int? Wins { get; set; }
        public int? WinsInRegulation { get; set; }
        public int? WinsInShootout { get; set; }
    }
}