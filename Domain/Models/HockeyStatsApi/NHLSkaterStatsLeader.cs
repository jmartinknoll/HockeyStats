namespace Domain.Models.HockeyStatsApi
{
    public class NHLSkaterStatsLeader
    {
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public string? TeamAbbreviation { get; set; }
        public string? Position { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? Points { get; set; }
        public double? PlusMinus { get; set; }
        public int? PenaltyMinutes { get; set; }
        // Add other relevant fields as needed
    }
}