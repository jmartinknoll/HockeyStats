using System.Text.Json;
using Domain.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.NHLClient.Endpoints
{
    public class NhlClient : INhlClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api-web.nhle.com/v1";
        private const string StatsUrl = "https://api.nhle.com/stats/rest";

        public NhlClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<T>> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return Result<T>.Failed($"Request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            //Console.WriteLine("=== RAW JSON RESPONSE ===");
            //Console.WriteLine(content);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(content);
                return Result<T>.Succeeded(result!);
            }
            catch (Exception ex)
            {
                return Result<T>.Failed($"Deserialization error: {ex.Message}");
            }
        }

        // API Reference: https://github.com/Zmalski/NHL-API-Reference?tab=readme-ov-file#get-team-stats

        /// <summary>
        /// limit of -1 will return all results
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Result<List<NHLSkaterStatsLeader>>> GetCurrentSkaterStatsLeaders(string? categories = null, int? limit = null)
        {
            var url = $"{BaseUrl}/skater-stats-leaders/current";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(categories)) queryParams.Add($"categories={categories}");
            if (limit.HasValue) queryParams.Add($"limit={limit}");
            if (queryParams.Any()) url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<List<NHLSkaterStatsLeader>>.Failed($"Request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("=== CURRENT SKATERS RAW RESPONSE ===");
            //Console.WriteLine(content);
            using var doc = JsonDocument.Parse(content);
            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return Result<List<NHLSkaterStatsLeader>>.Failed("Missing 'data' property.");
            }

            var stats = JsonConvert.DeserializeObject<List<NHLSkaterStatsLeader>>(dataElement.GetRawText());
            return stats != null
                ? Result<List<NHLSkaterStatsLeader>>.Succeeded(stats)
                : Result<List<NHLSkaterStatsLeader>>.Failed("Error deserializing data.");
        }

        /// <summary>
        /// Season in YYYYYYYY format, 
        /// where the first four digits represent the start year of the season, 
        /// and the last four digits represent the end year.
        /// Game type (2 for regular season, 3 for playoffs)
        /// limit of -1 will return all results
        /// </summary>
        /// <param name="season"></param>
        /// <param name="gameType"></param>
        /// <param name="categories"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Result<List<NHLSkaterStatsLeader>>> GetSkaterStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null)
        {
            var url = $"{BaseUrl}/skater-stats-leaders/{season}/{gameType}";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(categories)) queryParams.Add($"categories={categories}");
            if (limit.HasValue) queryParams.Add($"limit={limit}");
            if (queryParams.Any()) url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<List<NHLSkaterStatsLeader>>.Failed($"Request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("=== SKATERS STATS BY SEASON RAW RESPONSE ===");
            //Console.WriteLine(content);
            using var doc = JsonDocument.Parse(content);
            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return Result<List<NHLSkaterStatsLeader>>.Failed("Missing 'data' property.");
            }

            var stats = JsonConvert.DeserializeObject<List<NHLSkaterStatsLeader>>(dataElement.GetRawText());
            return stats != null
                ? Result<List<NHLSkaterStatsLeader>>.Succeeded(stats)
                : Result<List<NHLSkaterStatsLeader>>.Failed("Error deserializing data.");
        }

        /// <summary>
        /// limit of -1 will return all results
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Result<List<NHLGoalieStatsLeader>>> GetCurrentGoalieStatsLeaders(string? categories = null, int? limit = null)
        {
            var url = $"{BaseUrl}/goalie-stats-leaders/current";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(categories)) queryParams.Add($"categories={categories}");
            if (limit.HasValue) queryParams.Add($"limit={limit}");
            if (queryParams.Any()) url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<List<NHLGoalieStatsLeader>>.Failed($"Request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return Result<List<NHLGoalieStatsLeader>>.Failed("Missing 'data' property.");
            }

            var stats = JsonConvert.DeserializeObject<List<NHLGoalieStatsLeader>>(dataElement.GetRawText());
            return stats != null
                ? Result<List<NHLGoalieStatsLeader>>.Succeeded(stats)
                : Result<List<NHLGoalieStatsLeader>>.Failed("Error deserializing data.");
        }

        /// <summary>
        /// Season in YYYYYYYY format, 
        /// where the first four digits represent the start year of the season, 
        /// and the last four digits represent the end year.
        /// Game type (2 for regular season, 3 for playoffs)
        /// limit of -1 will return all results
        /// </summary>
        /// <param name="season"></param>
        /// <param name="gameType"></param>
        /// <param name="categories"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<Result<List<NHLGoalieStatsLeader>>> GetGoalieStatsLeadersBySeason(string season, int gameType, string? categories = null, int? limit = null)
        {
            // Build the URL with base path and route values
            var url = $"{BaseUrl}/goalie-stats-leaders/{season}/{gameType}";

            // Build optional query parameters
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(categories)) queryParams.Add($"categories={categories}");
            if (limit.HasValue) queryParams.Add($"limit={limit}");
            if (queryParams.Any()) url += "?" + string.Join("&", queryParams);

            // Make the GET request
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<List<NHLGoalieStatsLeader>>.Failed($"Request failed with status code {response.StatusCode}");
            }

            // Parse JSON response
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);

            // Extract "data" property
            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return Result<List<NHLGoalieStatsLeader>>.Failed("Missing 'data' property.");
            }

            // Deserialize into GoalieStatsLeader list
            var stats = JsonConvert.DeserializeObject<List<NHLGoalieStatsLeader>>(dataElement.GetRawText());

            // Return result
            return stats != null
                ? Result<List<NHLGoalieStatsLeader>>.Succeeded(stats)
                : Result<List<NHLGoalieStatsLeader>>.Failed("Error deserializing data.");
        }

        /// <summary>
        /// Team - Three-letter team code
        /// Season in YYYYYYYY format, 
        /// where the first four digits represent the start year of the season, 
        /// and the last four digits represent the end year.
        /// Game type (2 for regular season, 3 for playoffs)
        /// </summary>
        /// <param name="team"></param>
        /// <param name="season"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        public async Task<Result<NHLClubStats>> GetClubStatsBySeasonAndGameType(string team, string season, int gameType)
        {
            var url = $"{BaseUrl}/club-stats/{team}/{season}/{gameType}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return Result<NHLClubStats>.Failed($"Request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine("=== CLUB STATS RAW RESPONSE ===");
            //Console.WriteLine(content);

            // Deserialize directly
            var stats = JsonConvert.DeserializeObject<NHLClubStats>(content);

            return stats != null
                ? Result<NHLClubStats>.Succeeded(stats)
                : Result<NHLClubStats>.Failed("Error deserializing club stats.");
        }

        /// <summary>
        /// parameters "limit" : "-1" will return all results
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="report"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<Result<List<NHLTeamStats>>> GetTeamStats(string lang, string report, Dictionary<string, string>? parameters = null)
        {
            // Build the URL with the provided parameters
            var url = $"https://api.nhle.com/stats/rest/{lang}/team/{report}";

            // Make the GET request
            HttpResponseMessage response;
            if (parameters != null && parameters.Any())
            {
                var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
                response = await _httpClient.GetAsync($"{url}?{queryString}");
            }
            else
            {
                response = await _httpClient.GetAsync(url);
            }

            // Check for a successful response
            if (!response.IsSuccessStatusCode)
            {
                return Result<List<NHLTeamStats>>.Failed("Request failed.");
            }

            // Parse the response body
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // Try to extract the "data" property
            if (!doc.RootElement.TryGetProperty("data", out var dataElement))
            {
                return Result<List<NHLTeamStats>>.Failed("Missing 'data' property.");
            }

            // Deserialize the data array into a list of TeamStats objects
            var teamStats = JsonConvert.DeserializeObject<List<NHLTeamStats>>(dataElement.GetRawText());


            // If deserialization failed, return a failure result
            if (teamStats == null)
            {
                return Result<List<NHLTeamStats>>.Failed("Error deserializing data.");
            }

            // Return the successfully deserialized data wrapped in a Result
            return Result<List<NHLTeamStats>>.Succeeded(teamStats);
        }
    }
}
