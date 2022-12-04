namespace AdventOfCode.Execution.Framework;

public class AdventOfCodeExecutionBase
{
    internal readonly ITestOutputHelper output;
    internal readonly string inputFileName;

    public AdventOfCodeExecutionBase(ITestOutputHelper output)
    {
        this.output = output;

        var aoc = GetType().GetCustomAttribute<AdventOfCodeAttribute>();
        inputFileName = @$"..\..\..\Inputs\{aoc.Year}\{aoc.Day}.txt";
    }
    
    public async Task<string[]> GetInputAsync() => await File.ReadAllLinesAsync(inputFileName);

    public async Task GetResult<T>(Func<string[], T> action)
    {
        var result = action(await GetInputAsync());
        output.WriteLine($"{result}");
    }
}