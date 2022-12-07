using AdventOfCode.API;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AdventOfCode.Execution.Framework;

public class AdventOfCodeExecutionBase
{
    internal readonly ITestOutputHelper output;
    internal readonly string inputFileName;
    private readonly AdventOfCodeClient client = new() { BaseDirectory = @$"..\..\..\Inputs" };
    private readonly Regex _eventMatch = new(@".*?_(?<year>\d{4})\.day(?<day>\d{1,2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public AdventOfCodeExecutionBase(ITestOutputHelper output)
    {
        this.output = output;

        var m = _eventMatch.Match(GetType().FullName);
        var (year, day) = (m.Groups["year"].Value, m.Groups["day"].Value);
        if (!string.IsNullOrWhiteSpace(year) && !string.IsNullOrWhiteSpace(day))
        {
            inputFileName = @$"..\..\..\Inputs\{year}\{day}.txt";

            if (!File.Exists(inputFileName))
            {
                client.Input(year, day);
            }
        }
    }
    
    public async Task<string[]> GetInputAsync() => await File.ReadAllLinesAsync(inputFileName);

    public async Task Solve<T>(Func<string[], T> action)
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        var result = action(await GetInputAsync());
        stopwatch.Stop();

        output.WriteLine($"Duration: {TimeSpan.FromTicks(stopwatch.ElapsedTicks).TotalMilliseconds,8:F3} ms");
        output.WriteLine($"Answer: {result,13}");
    }

    public async Task SolveAsync<T>(Func<string[], Task<T>> asyncAction)
    {
        var result = await asyncAction(await GetInputAsync());
        output.WriteLine($"{result}");
    }
}