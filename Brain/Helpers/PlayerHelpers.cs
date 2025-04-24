using Domain.Models;
using Domain.Models.HockeyStatsApi;

namespace Brain.Helpers
{
    public class PlayerHelpers
    {
        public static void ProjectPlayerPoints(NHLSkater skaterInput, Player player, double playoffGames)
        {
            if (skaterInput.GamesPlayed == null || skaterInput.Points == null ||
                skaterInput.Goals == null || skaterInput.Assists == null || playoffGames <= 0)
            {
                player.ProjectedGoals = 0;
                player.ProjectedAssists = 0;
                return;
            }

            int points = skaterInput.Points.Value;
            int gamesPlayed = skaterInput.GamesPlayed.Value;

            double pointsPerGame = (double)points / gamesPlayed;
            double projectedPoints = pointsPerGame * playoffGames;

            int goals = skaterInput.Goals.Value;
            int assists = skaterInput.Assists.Value;

            double goalRatio = (double)goals / points;
            double assistRatio = (double)assists / points;

            player.ProjectedGoals = (int)Math.Round(projectedPoints * goalRatio);
            player.ProjectedAssists = (int)Math.Round(projectedPoints * assistRatio);

            // Optional: store original season stats
            player.GamesPlayed = skaterInput.GamesPlayed;
            player.Goals = skaterInput.Goals;
            player.Assists = skaterInput.Assists;
            player.Points = skaterInput.Points;
            player.SourceStats = skaterInput;
        }


        public static void ProjectGoalieStats(NHLGoalie goalieInput, Goalie goalie, double teamPlayoffWins, double teamPlayoffGames)
        {
            if (goalieInput.GamesPlayed == null || goalieInput.Shutouts == null || teamPlayoffGames <= 0)
            {
                goalie.ProjectedWins = 0;
                goalie.ProjectedShutouts = 0;
                return;
            }

            double goalieGameShare = (double)goalieInput.GamesPlayed / 82.0;
            double shutoutRatio = (double)goalieInput.Shutouts / goalieInput.GamesPlayed.Value;

            goalie.ProjectedWins = (int)Math.Round(teamPlayoffWins * goalieGameShare);
            goalie.ProjectedShutouts = (int)Math.Round(teamPlayoffGames * goalieGameShare * shutoutRatio);

            // Optional: store original season stats
            goalie.GamesPlayed = goalieInput.GamesPlayed;
            goalie.Wins = goalieInput.Wins;
            goalie.Shutouts = goalieInput.Shutouts;
            goalie.SourceStats = goalieInput;
        }


        public static List<Player> playersInPool = new List<Player>()
        {
            new() { FirstName = "Connor", LastName = "McDavid", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "F" },
            new() { FirstName = "Kyle", LastName = "Connor", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "F" },
            new() { FirstName = "Mikko", LastName = "Rantanen", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Mitch", LastName = "Marner", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "F" },
            new() { FirstName = "Nikita", LastName = "Kucherov", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "F" },
            new() { FirstName = "Alex", LastName = "Ovechkin", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Jason", LastName = "Robertson", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Leon", LastName = "Draisaitl", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "F" },
            new() { FirstName = "Nathan", LastName = "MacKinnon", Team = "Colorado Avalanche", TeamCode = "COL", Position = "F" },
            new() { FirstName = "Sam", LastName = "Reinhart", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Jack", LastName = "Eichel", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "F" },
            new() { FirstName = "Jesper", LastName = "Bratt", Team = "New Jersey Devils", TeamCode = "NJD", Position = "F" },
            new() { FirstName = "Martin", LastName = "Necas", Team = "Colorado Avalanche", TeamCode = "COL", Position = "F" },
            new() { FirstName = "Nick", LastName = "Suzuki", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "F" },
            new() { FirstName = "Robert", LastName = "Thomas", Team = "St. Louis Blues", TeamCode = "STL", Position = "F" },
            new() { FirstName = "Kirill", LastName = "Kaprizov", Team = "Minnesota Wild", TeamCode = "MIN", Position = "F" },
            new() { FirstName = "Mark", LastName = "Scheifele", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "F" },
            new() { FirstName = "Matt", LastName = "Duchene", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Sebastian", LastName = "Aho", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "F" },
            new() { FirstName = "Tim", LastName = "Stützle", Team = "Ottawa Senators", TeamCode = "OTT", Position = "F" },
            new() { FirstName = "Aleksander", LastName = "Barkov", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Auston", LastName = "Matthews", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "F" },
            new() { FirstName = "Brandon", LastName = "Hagel", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "F" },
            new() { FirstName = "Jordan", LastName = "Kyrou", Team = "St. Louis Blues", TeamCode = "STL", Position = "F" },
            new() { FirstName = "Wyatt", LastName = "Johnston", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Adrian", LastName = "Kempe", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "F" },
            new() { FirstName = "Brayden", LastName = "Point", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "F" },
            new() { FirstName = "Cole", LastName = "Caufield", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "F" },
            new() { FirstName = "Dylan", LastName = "Strome", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Nikolaj", LastName = "Ehlers", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "F" },
            new() { FirstName = "Anze", LastName = "Kopitar", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "F" },
            new() { FirstName = "Mark", LastName = "Stone", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "F" },
            new() { FirstName = "Matt", LastName = "Boldy", Team = "Minnesota Wild", TeamCode = "MIN", Position = "F" },
            new() { FirstName = "Seth", LastName = "Jarvis", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "F" },
            new() { FirstName = "William", LastName = "Nylander", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "F" },
            new() { FirstName = "Brady", LastName = "Tkachuk", Team = "Ottawa Senators", TeamCode = "OTT", Position = "F" },
            new() { FirstName = "Dylan", LastName = "Holloway", Team = "St. Louis Blues", TeamCode = "STL", Position = "F" },
            new() { FirstName = "Jake", LastName = "Guentzel", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "F" },
            new() { FirstName = "Roope", LastName = "Hintz", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Tomas", LastName = "Hertl", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "F" },
            new() { FirstName = "Aliaksei", LastName = "Protas", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Andrei", LastName = "Svechnikov", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "F" },
            new() { FirstName = "Brock", LastName = "Nelson", Team = "Colorado Avalanche", TeamCode = "COL", Position = "F" },
            new() { FirstName = "Nico", LastName = "Hischier", Team = "New Jersey Devils", TeamCode = "NJD", Position = "F" },
            new() { FirstName = "Ryan", LastName = "Nugent-Hopkins", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "F" },
            new() { FirstName = "Drake", LastName = "Batherson", Team = "Ottawa Senators", TeamCode = "OTT", Position = "F" },
            new() { FirstName = "John", LastName = "Tavares", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "F" },
            new() { FirstName = "Kevin", LastName = "Fiala", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "F" },
            new() { FirstName = "Marco", LastName = "Rossi", Team = "Minnesota Wild", TeamCode = "MIN", Position = "F" },
            new() { FirstName = "Matthew", LastName = "Tkachuk", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Gabriel", LastName = "Vilardi", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "F" },
            new() { FirstName = "Mikael", LastName = "Granlund", Team = "Dallas Stars", TeamCode = "DAL", Position = "F" },
            new() { FirstName = "Patrik", LastName = "Laine", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "F" },
            new() { FirstName = "Pavel", LastName = "Buchnevich", Team = "St. Louis Blues", TeamCode = "STL", Position = "F" },
            new() { FirstName = "Tom", LastName = "Wilson", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Anthony", LastName = "Cirelli", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "F" },
            new() { FirstName = "Brad", LastName = "Marchand", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Ivan", LastName = "Barbashev", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "F" },
            new() { FirstName = "Mats", LastName = "Zuccarello", Team = "Minnesota Wild", TeamCode = "MIN", Position = "F" },
            new() { FirstName = "Pierre-Luc", LastName = "Dubois", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Connor", LastName = "McMichael", Team = "Washington Capitals", TeamCode = "WSH", Position = "F" },
            new() { FirstName = "Jonathan", LastName = "Drouin", Team = "Colorado Avalanche", TeamCode = "COL", Position = "F" },
            new() { FirstName = "Matthew", LastName = "Knies", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "F" },
            new() { FirstName = "Sam", LastName = "Bennett", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Zach", LastName = "Hyman", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "F" },
            new() { FirstName = "Claude", LastName = "Giroux", Team = "Ottawa Senators", TeamCode = "OTT", Position = "F" },
            new() { FirstName = "Joel", LastName = "Eriksson Ek", Team = "Minnesota Wild", TeamCode = "MIN", Position = "F" },
            new() { FirstName = "Quinton", LastName = "Byfield", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "F" },
            new() { FirstName = "Taylor", LastName = "Hall", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "F" },
            new() { FirstName = "Timo", LastName = "Meier", Team = "New Jersey Devils", TeamCode = "NJD", Position = "F" },
            new() { FirstName = "Carter", LastName = "Verhaeghe", Team = "Florida Panthers", TeamCode = "FLA", Position = "F" },
            new() { FirstName = "Cole", LastName = "Perfetti", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "F" },
            new() { FirstName = "Dawson", LastName = "Mercer", Team = "New Jersey Devils", TeamCode = "NJD", Position = "F" },
            new() { FirstName = "Juraj", LastName = "Slafkovský", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "F" },
            new() { FirstName = "Valeri", LastName = "Nichushkin", Team = "Colorado Avalanche", TeamCode = "COL", Position = "F" },
            new() { FirstName = "Cale", LastName = "Makar", Team = "Colorado Avalanche", TeamCode = "COL", Position = "D" },
            new() { FirstName = "Evan", LastName = "Bouchard", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "D" },
            new() { FirstName = "John", LastName = "Carlson", Team = "Washington Capitals", TeamCode = "WSH", Position = "D" },
            new() { FirstName = "Shea", LastName = "Theodore", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "D" },
            new() { FirstName = "Victor", LastName = "Hedman", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "D" },
            new() { FirstName = "Jake", LastName = "Sanderson", Team = "Ottawa Senators", TeamCode = "OTT", Position = "D" },
            new() { FirstName = "Josh", LastName = "Morrissey", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "D" },
            new() { FirstName = "Lane", LastName = "Hutson", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "D" },
            new() { FirstName = "Shayne", LastName = "Gostisbehere", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "D" },
            new() { FirstName = "Thomas", LastName = "Harley", Team = "Dallas Stars", TeamCode = "DAL", Position = "D" },
            new() { FirstName = "Aaron", LastName = "Ekblad", Team = "Florida Panthers", TeamCode = "FLA", Position = "D" },
            new() { FirstName = "Cam", LastName = "Fowler", Team = "St. Louis Blues", TeamCode = "STL", Position = "D" },
            new() { FirstName = "Dougie", LastName = "Hamilton", Team = "New Jersey Devils", TeamCode = "NJD", Position = "D" },
            new() { FirstName = "Drew", LastName = "Doughty", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "D" },
            new() { FirstName = "Morgan", LastName = "Rielly", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "D" }
        };


        public static List<Goalie> goaliesInPool = new List<Goalie>()
        {
            new() { FirstName = "Andrei", LastName = "Vasilevskiy", Team = "Tampa Bay Lightning", TeamCode = "TBL", Position = "G" },
            new() { FirstName = "Connor", LastName = "Hellebuyck", Team = "Winnipeg Jets", TeamCode = "WPG", Position = "G" },
            new() { FirstName = "Jake", LastName = "Oettinger", Team = "Dallas Stars", TeamCode = "DAL", Position = "G" },
            new() { FirstName = "Logan", LastName = "Thompson", Team = "Washington Capitals", TeamCode = "WSH", Position = "G" },
            new() { FirstName = "Sergei", LastName = "Bobrovsky", Team = "Florida Panthers", TeamCode = "FLA", Position = "G" },
            new() { FirstName = "Adin", LastName = "Hill", Team = "Vegas Golden Knights", TeamCode = "VGK", Position = "G" },
            new() { FirstName = "Jordan", LastName = "Binnington", Team = "St. Louis Blues", TeamCode = "STL", Position = "G" },
            new() { FirstName = "Joseph", LastName = "Woll", Team = "Toronto Maple Leafs", TeamCode = "TOR", Position = "G" },
            new() { FirstName = "Mackenzie", LastName = "Blackwood", Team = "Colorado Avalanche", TeamCode = "COL", Position = "G" },
            new() { FirstName = "Pyotr", LastName = "Kochetkov", Team = "Carolina Hurricanes", TeamCode = "CAR", Position = "G" },
            new() { FirstName = "Darcy", LastName = "Kuemper", Team = "Los Angeles Kings", TeamCode = "LAK", Position = "G" },
            new() { FirstName = "Filip", LastName = "Gustavsson", Team = "Minnesota Wild", TeamCode = "MIN", Position = "G" },
            new() { FirstName = "Linus", LastName = "Ullmark", Team = "Ottawa Senators", TeamCode = "OTT", Position = "G" },
            new() { FirstName = "Sam", LastName = "Montembeault", Team = "Montréal Canadiens", TeamCode = "MTL", Position = "G" },
            new() { FirstName = "Stuart", LastName = "Skinner", Team = "Edmonton Oilers", TeamCode = "EDM", Position = "G" }
        };
    }
}
