namespace Domain.Interfaces
{
    public interface INhlClient
    {
        Task<Result<List<SkaterStatsLeader>>> GetCurrentSkaterStatsLeaders(string? categories = null, int? limit = null);
        Task<Result<List<SkaterStatsLeader>>> GetSkaterStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null);
        Task<Result<List<GoalieStatsLeader>>> GetCurrentGoalieStatsLeaders(string? categories = null, int? limit = null);
        Task<Result<List<GoalieStatsLeader>>> GetGoalieStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null);
        Task<Result<ClubStats>> GetClubStatsBySeasonAndGameType(string team, string season, int gameType);
        Task<Result<List<TeamStats>>> GetTeamStats(string lang = "en", string report = "summary", Dictionary<string, string>? queryParams = null);
    }
}
