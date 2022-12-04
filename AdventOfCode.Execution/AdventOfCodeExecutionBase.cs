using Xunit.Abstractions;

namespace AdventOfCode.Execution;

public class AdventOfCodeExecutionBase
{
    internal readonly ITestOutputHelper output;

    public AdventOfCodeExecutionBase(ITestOutputHelper output)
    {
        this.output = output;
    }
}