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

public class Pair
{
    public Pair(string value)
    {
        var pairs = value.Split(',')
            .Select(p => p.Split('-'))
            .Select(x => new Range(int.Parse(x[0]), int.Parse(x[1])))
            .ToList();
        (First, Second) = (pairs[0], pairs[1]);
    }

    public Range First { get; set; }
    public Range Second { get; set; }

    public bool FullyContains()
    {
        return (First.Start.Value <= Second.Start.Value && First.End.Value >= Second.End.Value) || (Second.Start.Value <= First.Start.Value && Second.End.Value >= First.End.Value);
    }

    public bool Overlaps()
    {
        return Enumerable.Range(First.Start.Value, First.End.Value - First.Start.Value + 1)
            .Intersect(Enumerable.Range(Second.Start.Value, Second.End.Value - Second.Start.Value + 1))
            .Count() > 0;
    }
}