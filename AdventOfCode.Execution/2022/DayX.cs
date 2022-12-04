using AdventOfCode2022.Day.Four;

namespace AdventOfCode.Execution._2022;

[AdventOfCode(2022, 0)]
public class DayX : AdventOfCodeExecutionBase
{
    public DayX(ITestOutputHelper output) : base(output)
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
        var result = GetResult<object>((lines) => default(object));
    }

    [Fact]
    public async void Part2_Execution()
    {
        var result = GetResult<object>((lines) => default(object));
    }
}