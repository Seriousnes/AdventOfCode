//using AdventOfCode2022.Day.Six;

namespace AdventOfCode.Execution._2022;

[AdventOfCode(2022, 0)]
public class Day6 : AdventOfCodeExecutionBase
{
    public Day6(ITestOutputHelper output) : base(output)
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
        await Solve<object>((lines) =>
        {
            return default(object);
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