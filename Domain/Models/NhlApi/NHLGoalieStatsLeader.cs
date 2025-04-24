namespace Domain.Models.NhlApi
{
    public class NHLGoalieStatsLeader
    {
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public string? TeamAbbreviation { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public double? GoalsAgainstAverage { get; set; }
        public double? SavePercentage { get; set; }
        public int? Shutouts { get; set; }
    }
}
