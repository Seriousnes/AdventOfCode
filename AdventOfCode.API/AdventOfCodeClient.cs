using static System.Net.WebRequestMethods;

namespace AdventOfCode.API;

public class AdventOfCodeClient
{    
    private static readonly HttpClient _httpClient = new(){ BaseAddress = new Uri("https://adventofcode.com/") };    

    public required string BaseDirectory { get; set; }

    /// <summary>
    /// Get the daily puzzle input
    /// </summary>
    /// <param name="year">Event year</param>
    /// <param name="day">Event Day</param>
    /// <returns></returns>
    public void Input(string year, string day)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{year}/day/{day}/input", UriKind.Relative)
        };
        message.Headers.Add("cookie", $"session={Session.Cookie}");
        var response = _httpClient.Send(message);

        if (response.IsSuccessStatusCode)
        {
            Directory.CreateDirectory(@$"{BaseDirectory}\{year}");
            using var fs = new FileStream(@$"{BaseDirectory}\{year}\{day}.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            response.Content.ReadAsStream().CopyTo(fs);
            fs.Close();         
        }
        else
        {
            throw new Exception($"Error {response.StatusCode} ({(int)response.StatusCode}) : \"{response.ReasonPhrase}\"");
        }        
    }
}
