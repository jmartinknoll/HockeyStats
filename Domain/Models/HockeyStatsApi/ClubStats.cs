using Domain.Models.HockeyStatsApi;

public class ClubStats
{
    public string? Season { get; set; }
    public int? GameType { get; set; }
    public List<Skater>? Skaters { get; set; }
    public List<Goalie>? Goalies { get; set; }
}