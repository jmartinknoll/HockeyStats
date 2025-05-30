﻿using Domain.Models.HockeyStatsCustom;
using Domain.Models.NhlApi;

namespace Brain.Helpers
{
    public class PlayoffHelpers
    {
        public static List<Team> SimulatePlayoffs(List<Team> teams, List<NHLTeamStats> statsList)
        {
            var eastern = teams.Where(t => t.Conference == "Eastern").ToList();
            var western = teams.Where(t => t.Conference == "Western").ToList();

            var eastChampion = SimulateConference(eastern, "Eastern", statsList);
            Console.WriteLine($"Eastern Champion: {eastChampion.Name}");
            var westChampion = SimulateConference(western, "Western", statsList);
            Console.WriteLine($"Western Champion: {westChampion.Name}");

            var champion = SimulateSeries(eastChampion, westChampion, 4, statsList);
            Console.WriteLine($"🏆 Stanley Cup Champion: {champion.Name}");

            foreach (var team in teams)
            {
                Console.WriteLine($"{team.Code} GP: {team.TotalGamesPlayed} W: {team.TotalWins} L: {team.TotalLosses} " +
                    $"GF: {team.TotalGoalsFor} GA: {team.TotalGoalsAgainst}");
            }

            return teams;
        }

        public static Team SimulateConference(List<Team> teams, string conference, List<NHLTeamStats> statsList)
        {
            // Filter teams by conference (Eastern or Western)
            var conferenceTeams = teams.Where(t => t.Conference == conference).OrderBy(t => t.Seed).ToList();

            // Group teams by division dynamically
            var divisions = conferenceTeams
                .GroupBy(t => t.Division)
                .ToDictionary(g => g.Key, g => g.OrderBy(t => t.Seed).ToList());

            // Make sure there are exactly two divisions in each conference
            if (divisions.Count != 2)
                throw new Exception($"Invalid division count for {conference} conference.");

            // Round 1 - First round matches for each division
            var division1Teams = divisions.Values.First();
            var division2Teams = divisions.Values.Last();

            var d1r1 = SimulateSeries(division1Teams[0], division1Teams[3], 1, statsList);
            var d1r2 = SimulateSeries(division1Teams[1], division1Teams[2], 1, statsList);
            var d2r1 = SimulateSeries(division2Teams[0], division2Teams[3], 1, statsList);
            var d2r2 = SimulateSeries(division2Teams[1], division2Teams[2], 1, statsList);

            // Round 2 - Division Finals
            var d1Winner = SimulateSeries(d1r1, d1r2, 2, statsList);
            var d2Winner = SimulateSeries(d2r1, d2r2, 2, statsList);

            // Round 3 - Conference Final
            return SimulateSeries(d1Winner, d2Winner, 3, statsList);
        }

        public static Team SimulateSeries(Team team1, Team team2, int round, List<NHLTeamStats> statsList)
        {
            Console.WriteLine($"Round {round}: {team1.Name} vs. {team2.Name}");
            int wins1 = 0, wins2 = 0;
            var rnd = new Random();

            var stats1 = statsList.FirstOrDefault(s => s.TeamFullName == team1.Name);
            var stats2 = statsList.FirstOrDefault(s => s.TeamFullName == team2.Name);

            var team1Stats = new RoundStats { RoundNumber = round };
            var team2Stats = new RoundStats { RoundNumber = round };

            while (wins1 < 4 && wins2 < 4)
            {
                var (winner, goals1, goals2) = SimulateGameWithScore(team1, team2, stats1!, stats2!);
                Console.WriteLine($"{winner.Name} W {goals1}-{goals2}");

                team1Stats.GamesPlayed++;
                team2Stats.GamesPlayed++;

                team1Stats.GoalsFor += goals1;
                team1Stats.GoalsAgainst += goals2;

                team2Stats.GoalsFor += goals2;
                team2Stats.GoalsAgainst += goals1;

                if (winner == team1)
                {
                    wins1++;
                    team1.TotalWins++;
                    team2.TotalLosses++;
                }
                else
                {
                    wins2++;
                    team2.TotalWins++;
                    team1.TotalLosses++;
                }
            }

            team1.TotalGamesPlayed += team1Stats.GamesPlayed;
            team1.TotalGoalsFor += team1Stats.GoalsFor;
            team1.TotalGoalsAgainst += team1Stats.GoalsAgainst;
            team1.RoundStats[round] = team1Stats;

            team2.TotalGamesPlayed += team2Stats.GamesPlayed;
            team2.TotalGoalsFor += team2Stats.GoalsFor;
            team2.TotalGoalsAgainst += team2Stats.GoalsAgainst;
            team2.RoundStats[round] = team2Stats;

            var seriesWinner = wins1 > wins2 ? team1 : team2;

            Console.WriteLine($"{seriesWinner.Name} wins Round {round}: {wins1}-{wins2}");

            return seriesWinner;
        }

        //public static double GetTeamStrength(NHLTeamStats stats)
        //{
        //    if (stats == null) return 0;

        //    double gpg = stats.GoalsForPerGame ?? 0;
        //    double gapg = stats.GoalsAgainstPerGame ?? 0;
        //    double pp = stats.PowerPlayPct ?? 0;
        //    double pk = stats.PenaltyKillPct ?? 0;
        //    double faceoff = stats.FaceoffWinPct ?? 0;
        //    double pointPct = stats.PointPct ?? 0;
        //    int regulationWins = stats.WinsInRegulation ?? 0;
        //    int gamesPlayed = stats.GamesPlayed ?? 82;

        //    // Normalize regulation wins to a 0–1 scale (if gamesPlayed > 0)
        //    double regulationWinRatio = gamesPlayed > 0 ? (double)regulationWins / gamesPlayed : 0;

        //    // Weighted strength calculation
        //    return
        //        (gpg * 2.0) -               // offense
        //        (gapg * 1.5) +              // defense
        //        (pp * 0.5) +                // power play
        //        (pk * 0.5) +                // penalty kill
        //        (faceoff * 0.25) +          // faceoffs
        //        (pointPct * 1.5) +          // overall performance
        //        (regulationWinRatio * 10);  // bonus for winning in regulation
        //}

        public static (Team winner, int goalsTeam1, int goalsTeam2) SimulateGameWithScore(Team team1, Team team2, NHLTeamStats stats1, NHLTeamStats stats2)
        {
            var rnd = new Random();

            // --- Unpack values with fallbacks ---
            double gpg1 = stats1.GoalsForPerGame ?? 3.0;
            double gapg1 = stats1.GoalsAgainstPerGame ?? 3.0;
            double shotsFor1 = stats1.ShotsForPerGame ?? 30.0;
            double shotsAgainst1 = stats1.ShotsAgainstPerGame ?? 30.0;
            double pp1 = stats1.PowerPlayPct ?? 20.0;
            double pk1 = stats1.PenaltyKillPct ?? 80.0;

            double gpg2 = stats2.GoalsForPerGame ?? 3.0;
            double gapg2 = stats2.GoalsAgainstPerGame ?? 3.0;
            double shotsFor2 = stats2.ShotsForPerGame ?? 30.0;
            double shotsAgainst2 = stats2.ShotsAgainstPerGame ?? 30.0;
            double pp2 = stats2.PowerPlayPct ?? 20.0;
            double pk2 = stats2.PenaltyKillPct ?? 80.0;

            double pim1 = team1.PenaltyMinutesPerGame ?? 10.0;
            double pim2 = team2.PenaltyMinutesPerGame ?? 10.0;

            // --- Additional factors ---
            double faceoffAdvantage1 = stats1.FaceoffWinPct ?? 50.0;
            double faceoffAdvantage2 = stats2.FaceoffWinPct ?? 50.0;
            double disciplineFactor1 = pim1 > 12.0 ? -0.2 : 0.0;
            double disciplineFactor2 = pim2 > 12.0 ? -0.2 : 0.0;

            // --- Calculate base offensive pressure via shots and goals ---
            double shotPressure1 = shotsFor1 - shotsAgainst2;
            double shotPressure2 = shotsFor2 - shotsAgainst1;

            double baseGoals1 = gpg1 + (shotPressure1 * 0.05) - (gapg2 * 0.2) + disciplineFactor1;
            double baseGoals2 = gpg2 + (shotPressure2 * 0.05) - (gapg1 * 0.2) + disciplineFactor2;

            // --- Estimate power play opportunities ---
            int ppChances1 = (int)Math.Round(pim2 / 2.0);
            int ppChances2 = (int)Math.Round(pim1 / 2.0);

            // --- Adjust for special teams ---
            double ppGoals1 = ppChances1 * (pp1 / 100.0) * ((100.0 - pk2) / 100.0);
            double ppGoals2 = ppChances2 * (pp2 / 100.0) * ((100.0 - pk1) / 100.0);

            // --- Total expected goals before randomness ---
            double expectedGoals1 = baseGoals1 + ppGoals1;
            double expectedGoals2 = baseGoals2 + ppGoals2;

            // Clamp expectations to realistic bounds
            expectedGoals1 = Math.Clamp(expectedGoals1, 1.0, 6.5);
            expectedGoals2 = Math.Clamp(expectedGoals2, 1.0, 6.5);

            // --- Add game-to-game randomness ---
            double randomness1 = rnd.NextDouble() * 1.5;
            double randomness2 = rnd.NextDouble() * 1.5;

            int finalGoals1 = (int)Math.Round(expectedGoals1 + randomness1);
            int finalGoals2 = (int)Math.Round(expectedGoals2 + randomness2);

            // Ensure no ties in playoffs
            while (finalGoals1 == finalGoals2)
            {
                finalGoals1 += rnd.Next(0, 2);
                finalGoals2 += rnd.Next(0, 2);
            }

            Team winner = finalGoals1 > finalGoals2 ? team1 : team2;
            return (winner, finalGoals1, finalGoals2);
        }

        public static double NormalDistribution(Random rng, double mean, double stdDev)
        {
            // Box-Muller transform
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        public static Dictionary<string, SimulationStats> RunMultipleSimulations(List<Team> teams, List<NHLTeamStats> statsList, int numberOfSimulations)
        {
            var results = new Dictionary<string, SimulationStats>();

            // Initialize the results dictionary for each team
            foreach (var team in teams)
            {
                results[team.Code] = new SimulationStats { TeamCode = team.Code, Simulations = numberOfSimulations };
            }

            // Run the simulations
            for (int i = 0; i < numberOfSimulations; i++)
            {
                // Clone teams to reset stats before each simulation
                var clonedTeams = teams.Select(team => team.Clone()).ToList();

                var simulatedTeams = SimulatePlayoffs(clonedTeams, statsList);  // Simulate the playoffs for each run

                foreach (var team in simulatedTeams)
                {
                    var stats = results[team.Code];

                    // Add the stats from this simulation run
                    stats.TotalGamesPlayed += team.TotalGamesPlayed;
                    stats.TotalWins += team.TotalWins;
                    stats.TotalLosses += team.TotalLosses;
                    stats.TotalGoalsFor += team.TotalGoalsFor;
                    stats.TotalGoalsAgainst += team.TotalGoalsAgainst;
                }
            }

            // After running all simulations, calculate averages
            foreach (var stats in results.Values)
            {
                stats.AvgGamesPlayed = stats.TotalGamesPlayed / (double)stats.Simulations;
                stats.AvgWins = stats.TotalWins / (double)stats.Simulations;
                stats.AvgLosses = stats.TotalLosses / (double)stats.Simulations;
                stats.AvgGoalsFor = stats.TotalGoalsFor / (double)stats.Simulations;
                stats.AvgGoalsAgainst = stats.TotalGoalsAgainst / (double)stats.Simulations;
            }

            return results;
        }
    }
}
