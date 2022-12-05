using AdventOfCode.API;
using System.Net.Http.Headers;

namespace AdventOfCode.Execution.Framework;

public class AdventOfCodeExecutionBase
{
    internal readonly ITestOutputHelper output;
    internal readonly string inputFileName;
    private readonly AdventOfCodeClient client = new() { BaseDirectory = @$"..\..\..\Inputs" };

    public AdventOfCodeExecutionBase(ITestOutputHelper output)
    {
        this.output = output;

        var aoc = GetType().GetCustomAttribute<AdventOfCodeAttribute>();
        inputFileName = @$"..\..\..\Inputs\{aoc.Year}\{aoc.Day}.txt";

        if (!File.Exists(inputFileName) && aoc is { })
        {
            client.Input(aoc.Year, aoc.Day);
        }
    }
    
    public async Task<string[]> GetInputAsync() => await File.ReadAllLinesAsync(inputFileName);

    public async Task Solve<T>(Func<string[], T> action)
    {
        var result = action(await GetInputAsync());
        output.WriteLine($"{result}");
    }

    public async Task SolveAsync<T>(Func<string[], Task<T>> asyncAction)
    {
        var result = await asyncAction(await GetInputAsync());
        output.WriteLine($"{result}");
    }
}