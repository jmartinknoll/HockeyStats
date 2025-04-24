namespace Domain.Models.HockeyStatsApi
{
    public class NHLStatLeader
    {
        public NHLName? FirstName { get; set; }
        public NHLName? LastName { get; set; }
        public string? Position { get; set; }
        public double? Value { get; set; }
    }
}
