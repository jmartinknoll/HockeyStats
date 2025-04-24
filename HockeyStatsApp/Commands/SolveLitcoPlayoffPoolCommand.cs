using MediatR;
using Domain.Interfaces;
using Brain.Helpers;
using Domain.Models;
using Domain.Models.HockeyStatsApi;

namespace HockeyStatsApp.Commands
{
    public class SolveLitcoPlayoffPoolCommand : IRequest<Result<List<NHLTeamStats>>>
    {
        //public
    }

    public class GetHockeyStatsQueryHandler : IRequestHandler<SolveLitcoPlayoffPoolCommand, Result<List<NHLTeamStats>>>
    {
        private readonly INhlClient _nhlClient;

        public GetHockeyStatsQueryHandler(INhlClient nhlClient)
        {
            _nhlClient = nhlClient;
        }

        public async Task<Result<List<NHLTeamStats>>> Handle(SolveLitcoPlayoffPoolCommand request, CancellationToken cancellationToken)
        {
            var playoffTeams = TeamHelpers.playoffTeams;
            var teamNames = playoffTeams.Select(t => t.Name).ToList();

            // Team Stats
            var queryParams = new Dictionary<string, string>();
            queryParams.Add("limit", "-1");
            var resTeamStats = await _nhlClient.GetTeamStats(queryParams: queryParams);
            if (!resTeamStats.Success)
            {
                return new Result<List<NHLTeamStats>>(false, default, resTeamStats.Code, $"Failed to retrieve team stats. Error: {resTeamStats.ErrorMessage}");
            }

            var teamStats = resTeamStats.Value.Where(t => t.SeasonId == "20242025" && teamNames.Contains(t.TeamFullName)).ToList();

            //var simulatedTeamStats = PlayoffHelpers.SimulatePlayoffs(playoffTeams, teamStats);

            var results = PlayoffHelpers.RunMultipleSimulations(playoffTeams, teamStats, 1000);

            foreach (var kvp in results.OrderBy(r => r.Key))
            {
                var stats = kvp.Value;
                Console.WriteLine($"{stats.TeamCode} (avg over {stats.Simulations} sims): " +
                    $"GP: {stats.AvgGamesPlayed:F1}, W: {stats.AvgWins:F1}, L: {stats.AvgLosses:F1}, " +
                    $"GF: {stats.AvgGoalsFor:F1}, GA: {stats.AvgGoalsAgainst:F1}");
            }

            Dictionary<string, NHLClubStats> clubStats = new Dictionary<string, NHLClubStats>();

            foreach (var team in teamStats)
            {
                var teamCode = TeamHelpers.teamCodes[team.TeamFullName];

                //Club Stats
                var resClubStats = await _nhlClient.GetClubStatsBySeasonAndGameType(teamCode, "20242025", 2);
                if (!resClubStats.Success)
                {
                    return new Result<List<NHLTeamStats>>(false, default, resClubStats.Code, $"Failed to retrieve club stats. Error: {resClubStats.ErrorMessage}");
                }

                clubStats[teamCode] = resClubStats.Value;
            }

            //var firstNames = clubStats["MTL"].Skaters.Select(s => s.FirstName.Default).ToList();
            //var lastNames = clubStats["MTL"].Skaters.Select(s => s.LastName.Default).ToList();
            var players = PlayerHelpers.playersInPool;

            foreach (var player in players)
            {
                var playerSeasonStats = clubStats[player.TeamCode].Skaters.Where(p => p.FirstName.Default == player.FirstName && p.LastName.Default == player.LastName).FirstOrDefault();
                var playoffGames = results[player.TeamCode].AvgGamesPlayed;
                PlayerHelpers.ProjectPlayerPoints(playerSeasonStats, player, playoffGames);
                Console.WriteLine($"{player.FirstName} {player.LastName} - G: {player.ProjectedGoals} A: {player.ProjectedAssists} P: {player.ProjectedGoals + player.ProjectedAssists}");
            }

            var goalies = PlayerHelpers.goaliesInPool;

            foreach (var goalie in goalies)
            {
                var goalieSeasonStats = clubStats[goalie.TeamCode].Goalies.Where(p => p.FirstName.Default == goalie.FirstName && p.LastName.Default == goalie.LastName).FirstOrDefault();
                var playoffGames = results[goalie.TeamCode].AvgGamesPlayed;
                var playoffWins = results[goalie.TeamCode].AvgWins;
                PlayerHelpers.ProjectGoalieStats(goalieSeasonStats, goalie, playoffWins, playoffGames);
                Console.WriteLine($"{goalie.FirstName} {goalie.LastName} - W: {goalie.ProjectedWins} SO: {goalie.ProjectedShutouts} P: {goalie.ProjectedWins + (2 * goalie.ProjectedShutouts)}");
            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Player Selections:\n");

            int chunkSize = 5;
            for (int i = 0; i < players.Count; i += chunkSize)
            {
                var chunk = players.Skip(i).Take(chunkSize)
                    .OrderByDescending(p => p.ProjectedGoals + p.ProjectedAssists)
                    .ToList();

                if (chunk.Count > 0)
                {
                    var topPlayer = chunk[0];
                    Console.WriteLine($"[Top Skater #{i / chunkSize + 1}] {topPlayer.FirstName} {topPlayer.LastName} - G: {topPlayer.ProjectedGoals} A: {topPlayer.ProjectedAssists} P: {topPlayer.ProjectedGoals + topPlayer.ProjectedAssists}");
                }

                if (chunk.Count > 1)
                {
                    var secondPlayer = chunk[1];
                    Console.WriteLine($"[Second Skater #{i / chunkSize + 1}] {secondPlayer.FirstName} {secondPlayer.LastName} - G: {secondPlayer.ProjectedGoals} A: {secondPlayer.ProjectedAssists} P: {secondPlayer.ProjectedGoals + secondPlayer.ProjectedAssists}");
                }
            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Goalie Selections:\n");

            for (int i = 0; i < goalies.Count; i += chunkSize)
            {
                var chunk = goalies.Skip(i).Take(chunkSize)
                    .OrderByDescending(g => g.ProjectedWins + 2 * g.ProjectedShutouts)
                    .ToList();

                if (chunk.Count > 0)
                {
                    var topGoalie = chunk[0];
                    Console.WriteLine($"[Top Goalie #{i / chunkSize + 1}] {topGoalie.FirstName} {topGoalie.LastName} - W: {topGoalie.ProjectedWins} SO: {topGoalie.ProjectedShutouts} P: {topGoalie.ProjectedWins + 2 * topGoalie.ProjectedShutouts}");
                }

                if (chunk.Count > 1)
                {
                    var secondGoalie = chunk[1];
                    Console.WriteLine($"[Second Goalie #{i / chunkSize + 1}] {secondGoalie.FirstName} {secondGoalie.LastName} - W: {secondGoalie.ProjectedWins} SO: {secondGoalie.ProjectedShutouts} P: {secondGoalie.ProjectedWins + 2 * secondGoalie.ProjectedShutouts}");
                }
            }



            //Skater Stats
            //var resSkaterStats = await _nhlClient.GetSkaterStatsLeadersBySeason("20242025", 2, limit: 1);
            //if (!resSkaterStats.Success)
            //{
            //    return new Result<List<NHLTeamStats>>(false, default, resSkaterStats.Code, $"Failed to retrieve club stats. Error: {resSkaterStats.ErrorMessage}");
            //}

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

            return new Result<List<NHLTeamStats>>(true, teamStats);
        }
    }
}
