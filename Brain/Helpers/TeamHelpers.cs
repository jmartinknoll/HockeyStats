using Domain.Models;

namespace Brain.Helpers
{
    public class TeamHelpers
    {
        public static List<Team> playoffTeams = new List<Team>()
        {
            new() { Name = "Toronto Maple Leafs", Code = "TOR", Conference = "Eastern", Division = "Atlantic", Seed = 1, PenaltyMinutesPerGame = 3.2 },
            new() { Name = "Tampa Bay Lightning", Code = "TBL", Conference = "Eastern", Division = "Atlantic", Seed = 2, PenaltyMinutesPerGame = 3.7 },
            new() { Name = "Florida Panthers", Code = "FLA", Conference = "Eastern", Division = "Atlantic", Seed = 3, PenaltyMinutesPerGame = 4.1 },
            new() { Name = "Washington Capitals", Code = "WSH", Conference = "Eastern", Division = "Metropolitan", Seed = 1, PenaltyMinutesPerGame = 3.5 },
            new() { Name = "Carolina Hurricanes", Code = "CAR", Conference = "Eastern", Division = "Metropolitan", Seed = 2, PenaltyMinutesPerGame = 2.9 },
            new() { Name = "New Jersey Devils", Code = "NJD", Conference = "Eastern", Division = "Metropolitan", Seed = 3, PenaltyMinutesPerGame = 3.0 },
            new() { Name = "Ottawa Senators", Code = "OTT", Conference = "Eastern", Division = "Atlantic", Seed = 4, PenaltyMinutesPerGame = 4.3 },
            new() { Name = "Montréal Canadiens", Code = "MTL", Conference = "Eastern", Division = "Metropolitan", Seed = 4, PenaltyMinutesPerGame = 3.8 },
            new() { Name = "Winnipeg Jets", Code = "WPG", Conference = "Western", Division = "Central", Seed = 1, PenaltyMinutesPerGame = 3.6 },
            new() { Name = "Dallas Stars", Code = "DAL", Conference = "Western", Division = "Central", Seed = 2, PenaltyMinutesPerGame = 3.1 },
            new() { Name = "Colorado Avalanche", Code = "COL", Conference = "Western", Division = "Central", Seed = 3, PenaltyMinutesPerGame = 3.3 },
            new() { Name = "Vegas Golden Knights", Code = "VGK", Conference = "Western", Division = "Pacific", Seed = 1, PenaltyMinutesPerGame = 3.4 },
            new() { Name = "Los Angeles Kings", Code = "LAK", Conference = "Western", Division = "Pacific", Seed = 2, PenaltyMinutesPerGame = 2.8 },
            new() { Name = "Edmonton Oilers", Code = "EDM", Conference = "Western", Division = "Pacific", Seed = 3, PenaltyMinutesPerGame = 3.2 },
            new() { Name = "Minnesota Wild", Code = "MIN", Conference = "Western", Division = "Pacific", Seed = 4, PenaltyMinutesPerGame = 4.0 },
            new() { Name = "St. Louis Blues", Code = "STL", Conference = "Western", Division = "Central", Seed = 4, PenaltyMinutesPerGame = 3.9 },
        };


        public static Dictionary<string, string> teamCodes = new Dictionary<string, string>
        {
            { "Anaheim Ducks", "ANA" },
            { "Arizona Coyotes", "ARI" },
            { "Boston Bruins", "BOS" },
            { "Buffalo Sabres", "BUF" },
            { "Calgary Flames", "CGY" },
            { "Carolina Hurricanes", "CAR" },
            { "Chicago Blackhawks", "CHI" },
            { "Colorado Avalanche", "COL" },
            { "Columbus Blue Jackets", "CBJ" },
            { "Dallas Stars", "DAL" },
            { "Detroit Red Wings", "DET" },
            { "Edmonton Oilers", "EDM" },
            { "Florida Panthers", "FLA" },
            { "Los Angeles Kings", "LAK" },
            { "Minnesota Wild", "MIN" },
            { "Montréal Canadiens", "MTL" },
            { "Nashville Predators", "NSH" },
            { "New Jersey Devils", "NJD" },
            { "New York Islanders", "NYI" },
            { "New York Rangers", "NYR" },
            { "Ottawa Senators", "OTT" },
            { "Philadelphia Flyers", "PHI" },
            { "Pittsburgh Penguins", "PIT" },
            { "San Jose Sharks", "SJS" },
            { "Seattle Kraken", "SEA" },
            { "St. Louis Blues", "STL" },
            { "Tampa Bay Lightning", "TBL" },
            { "Toronto Maple Leafs", "TOR" },
            { "Vancouver Canucks", "VAN" },
            { "Vegas Golden Knights", "VGK" },
            { "Washington Capitals", "WSH" },
            { "Winnipeg Jets", "WPG" }
        };
    }
}
