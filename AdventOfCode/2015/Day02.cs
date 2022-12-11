//using AdventOfCode2022.Day.X;

using System.Net.Mime;

namespace AdventOfCode.Execution._2015;

public class Day02 : AdventOfCodeExecutionBase
{
    public Day02(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part1_Validation(string input, int expectedValue)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, int expectedValue)
    {
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var sqrFt = 0;
            foreach (var line in lines)
            {
                var sides = line.Split('x').Select(int.Parse).ToArray();
                var (l, w, h) = (sides[0], sides[1], sides[2]);

                sqrFt += (2 * l * w) + (2 * h * w) + (2 * h * l) + new[] { l * w, l * h, h * w }.Min();
            }

            return sqrFt;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var distance = 0;
            foreach (var line in lines)
            {
                var sides = line.Split('x').Select(int.Parse).ToArray();
                var (l, w, h) = (sides[0], sides[1], sides[2]);

                distance += (l * w * h) + (new[] { l, h, w }.OrderBy(x => x).Take(2).Sum() * 2);
            }

            return distance;
        });
    }
}