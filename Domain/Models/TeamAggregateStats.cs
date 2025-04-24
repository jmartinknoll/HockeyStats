namespace Domain.Models
{
    public class TeamAggregateStats
    {
        public string TeamCode { get; set; }
        public int Simulations { get; set; } = 0;
        public int TotalGamesPlayed { get; set; } = 0;
        public int TotalWins { get; set; } = 0;
        public int TotalLosses { get; set; } = 0;
        public int TotalGoalsFor { get; set; } = 0;
        public int TotalGoalsAgainst { get; set; } = 0;

        public void AddSimulationResult(Team team)
        {
            Simulations++;
            TotalGamesPlayed += team.TotalGamesPlayed;
            TotalWins += team.TotalWins;
            TotalLosses += team.TotalLosses;
            TotalGoalsFor += team.TotalGoalsFor;
            TotalGoalsAgainst += team.TotalGoalsAgainst;
        }

        public double AvgGamesPlayed => (double)TotalGamesPlayed / Simulations;
        public double AvgWins => (double)TotalWins / Simulations;
        public double AvgLosses => (double)TotalLosses / Simulations;
        public double AvgGoalsFor => (double)TotalGoalsFor / Simulations;
        public double AvgGoalsAgainst => (double)TotalGoalsAgainst / Simulations;
    }
}
