using MediatR;
using Domain.Interfaces;
using Brain.Helpers;

namespace HockeyStatsApp.Queries
{
    public class GetHockeyStatsQuery : IRequest<Result<List<TeamStats>>>
    {
        //public
    }

    public class GetHockeyStatsQueryHandler : IRequestHandler<GetHockeyStatsQuery, Result<List<TeamStats>>>
    {
        private readonly INhlClient _nhlClient;

        public GetHockeyStatsQueryHandler(INhlClient nhlClient)
        {
            _nhlClient = nhlClient;
        }

        public async Task<Result<List<TeamStats>>> Handle(GetHockeyStatsQuery request, CancellationToken cancellationToken)
        {
            var playoffTeams = TeamHelpers.playoffTeams;
            var teamNames = playoffTeams.Select(t => t.Name).ToList();

            // Team Stats
            var queryParams = new Dictionary<string, string>();
            queryParams.Add("limit", "-1");
            var resTeamStats = await _nhlClient.GetTeamStats(queryParams: queryParams);
            if (!resTeamStats.Success)
            {
                return new Result<List<TeamStats>>(false, default, resTeamStats.Code, $"Failed to retrieve team stats. Error: {resTeamStats.ErrorMessage}");
            }

            var teamStats = resTeamStats.Value.Where(t => t.SeasonId == "20242025" && teamNames.Contains(t.TeamFullName)).ToList();

            PlayoffHelpers.SimulatePlayoffs(playoffTeams, teamStats);

            Dictionary<string, ClubStats> clubStats = new Dictionary<string, ClubStats>();

            foreach (var team in teamStats)
            {
                var teamCode = TeamHelpers.teamCodes[team.TeamFullName];

                //Club Stats
                var resClubStats = await _nhlClient.GetClubStatsBySeasonAndGameType(teamCode, "20242025", 2);
                if (!resClubStats.Success)
                {
                    return new Result<List<TeamStats>>(false, default, resClubStats.Code, $"Failed to retrieve club stats. Error: {resClubStats.ErrorMessage}");
                }

                clubStats[teamCode] = resClubStats.Value;
            }


            //Skater Stats
            var resSkaterStats = await _nhlClient.GetSkaterStatsLeadersBySeason("20242025", 2, limit: 1);
            if (!resSkaterStats.Success)
            {
                return new Result<List<TeamStats>>(false, default, resSkaterStats.Code, $"Failed to retrieve club stats. Error: {resSkaterStats.ErrorMessage}");
            }

            //var resCurrentSkaterStats = await _nhlClient.GetCurrentSkaterStatsLeaders(limit: 2);
            //if (!resCurrentSkaterStats.Success)
            //{
            //    return new Result<List<TeamStats>>(false, default, resCurrentSkaterStats.Code, $"Failed to retrieve club stats. Error: {resCurrentSkaterStats.ErrorMessage}");
            //}

            ////Goalie Stats
            //var resGoalieStats = await _nhlClient.GetGoalieStatsLeadersBySeason("20242025", 2, limit: 2);
            //if (!resGoalieStats.Success)
            //{
            //    return new Result<List<TeamStats>>(false, default, resGoalieStats.Code, $"Failed to retrieve club stats. Error: {resGoalieStats.ErrorMessage}");
            //}

            //var resCurrentGoalieStats = await _nhlClient.GetCurrentGoalieStatsLeaders(limit: 2);
            //if (!resCurrentGoalieStats.Success)
            //{
            //    return new Result<List<TeamStats>>(false, default, resCurrentGoalieStats.Code, $"Failed to retrieve club stats. Error: {resCurrentGoalieStats.ErrorMessage}");
            //}

            var nMackinnonPoints = clubStats["COL"].Skaters?.Where(s => s.LastName.Default == "MacKinnon").Select(s => s.Points).FirstOrDefault();






            return new Result<List<TeamStats>>(true, teamStats);
        }
    }
}
