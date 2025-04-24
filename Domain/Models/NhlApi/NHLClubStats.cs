namespace Domain.Models.NhlApi
{
    public class NHLClubStats
    {
        public string? Season { get; set; }
        public int? GameType { get; set; }
        public List<NHLSkater>? Skaters { get; set; }
        public List<NHLGoalie>? Goalies { get; set; }
    }
}
