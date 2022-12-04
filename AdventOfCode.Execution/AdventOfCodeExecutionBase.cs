using Xunit.Abstractions;

namespace AdventOfCode.Execution;

public class AdventOfCodeExecutionBase
{
    internal readonly ITestOutputHelper output;
    internal readonly string inputFileName;

    public AdventOfCodeExecutionBase(ITestOutputHelper output, string inputFileName)
    {
        this.output = output;
        this.inputFileName = inputFileName;
    }

    internal async Task<string[]> GetInput() => await File.ReadAllLinesAsync(@$"..\..\..\Inputs\2022\{inputFileName}.txt");
}