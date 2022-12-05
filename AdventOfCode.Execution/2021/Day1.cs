//using AdventOfCode2022.Day.X;

namespace AdventOfCode.Execution._2021;

[AdventOfCode(2021, 1)]
public class Day1 : AdventOfCodeExecutionBase
{
    public Day1(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part1_Validation(string input, object expectedValue)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, object expectedValue)
    {        
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve((lines) =>
        {
            int? lastDepth = null;
            int depthIncreased = 0;
            foreach (var depth in lines.Select(x => int.Parse(x)))
            {
                if (lastDepth.HasValue && depth > lastDepth)
                    depthIncreased++;
                lastDepth = depth;
            }
            return depthIncreased;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            return default(object);
        });
    }
}