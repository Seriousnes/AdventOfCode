using AdventOfCode2022.Day.Four;

namespace AdventOfCode.Execution._2022;

public class Day4 : AdventOfCodeExecutionBase
{
    public Day4(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData()]
    public void Part1_Validation()
    {
    }

    [Theory]
    [InlineData(@"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8", 4)]
    public void Part2_Validation(string input, int expectedOverlaps)
    {
        input.Split(Environment.NewLine)
            .Select(x => new Pair(x))
            .Count(x => x.Overlaps())
            .Should().Be(expectedOverlaps);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve(lines => lines.Select(p => new Pair(p)).Count(x => x.FullyContains()));
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve(lines => lines.Select(p => new Pair(p)).Count(x => x.Overlaps()));
    }
}