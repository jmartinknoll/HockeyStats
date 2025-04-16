namespace Domain.Interfaces
{
    public interface INhlClient
    {
        Task<Result<List<NHLSkaterStatsLeader>>> GetCurrentSkaterStatsLeaders(string? categories = null, int? limit = null);
        Task<Result<List<NHLSkaterStatsLeader>>> GetSkaterStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null);
        Task<Result<List<NHLGoalieStatsLeader>>> GetCurrentGoalieStatsLeaders(string? categories = null, int? limit = null);
        Task<Result<List<NHLGoalieStatsLeader>>> GetGoalieStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null);
        Task<Result<NHLClubStats>> GetClubStatsBySeasonAndGameType(string team, string season, int gameType);
        Task<Result<List<NHLTeamStats>>> GetTeamStats(string lang = "en", string report = "summary", Dictionary<string, string>? queryParams = null);
    }
}
