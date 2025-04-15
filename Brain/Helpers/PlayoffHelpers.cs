using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Brain.Helpers
{
    public class PlayoffHelpers
    {
        public static void SimulatePlayoffs(List<Team> teams, List<TeamStats> statsList)
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
                Console.WriteLine($"{team.Name} Games Played: {team.TotalGamesPlayed} Goals For: {team.TotalGoalsFor} Goals Against: {team.TotalGoalsAgainst}");
            }
        }

        public static Team SimulateSeries(Team team1, Team team2, int round, List<TeamStats> statsList)
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

                if (winner == team1) wins1++;
                else wins2++;
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



        public static Team SimulateConference(List<Team> teams, string conference, List<TeamStats> statsList)
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


        public static double GetTeamStrength(TeamStats stats)
        {
            if (stats == null) return 0;

            double gpg = stats.GoalsForPerGame ?? 0;
            double gapg = stats.GoalsAgainstPerGame ?? 0;
            double pp = stats.PowerPlayPct ?? 0;
            double pk = stats.PenaltyKillPct ?? 0;
            double faceoff = stats.FaceoffWinPct ?? 0;
            double pointPct = stats.PointPct ?? 0;

            return (gpg * 2) - (gapg * 1.5) + (pp * 0.5) + (pk * 0.5) + (faceoff * 0.25) + (pointPct * 1.5);
        }

        //public static Team SimulateGame(Team teamA, Team teamB, TeamStats statsA, TeamStats statsB)
        //{
        //    Random rng = new();

        //    double strengthA = GetTeamStrength(statsA);
        //    double strengthB = GetTeamStrength(statsB);
        //    double total = strengthA + strengthB;
        //    double chanceA = strengthA / total;

        //    Console.WriteLine($"Simulating: {teamA.Name} (Strength: {strengthA:F2}) vs {teamB.Name} (Strength: {strengthB:F2})");
        //    Console.WriteLine($"Chance {teamA.Name} wins: {(chanceA * 100):F1}%");

        //    return rng.NextDouble() < chanceA ? teamA : teamB;
        //}

        // Helper method to simulate the goal scoring based on attack and defense strength
        //private static int SimulateGoals(double attackStrength, double defenseStrength, Random rng)
        //{
        //    // The idea is to simulate goals scored using attack strength vs. defense strength
        //    double expectedGoals = attackStrength / (defenseStrength + 1);  // +1 to avoid division by zero
        //    int goals = Math.Max(0, (int)Math.Round(rng.NextDouble() * expectedGoals * 2)); // Randomize, but within a reasonable range
        //    return goals;
        //}

        public static (Team winner, int goalsA, int goalsB) SimulateGameWithScore(Team teamA, Team teamB, TeamStats statsA, TeamStats statsB)
        {
            var rng = new Random();

            // Use GoalsForPerGame and GoalsAgainstPerGame to estimate expected goals
            double expectedA = ((statsA.GoalsForPerGame ?? 3.0) + (statsB.GoalsAgainstPerGame ?? 3.0)) / 2;
            double expectedB = ((statsB.GoalsForPerGame ?? 3.0) + (statsA.GoalsAgainstPerGame ?? 3.0)) / 2;

            // Add some randomness
            int goalsA = Math.Max(0, (int)Math.Round(NormalDistribution(rng, expectedA, 1.0)));
            int goalsB = Math.Max(0, (int)Math.Round(NormalDistribution(rng, expectedB, 1.0)));

            // Overtime logic (just pick a winner in a tie)
            if (goalsA == goalsB)
            {
                if (rng.NextDouble() < 0.5) goalsA++;
                else goalsB++;
            }

            return (goalsA > goalsB ? teamA : teamB, goalsA, goalsB);
        }

        public static double NormalDistribution(Random rng, double mean, double stdDev)
        {
            // Box-Muller transform
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

    }
}
