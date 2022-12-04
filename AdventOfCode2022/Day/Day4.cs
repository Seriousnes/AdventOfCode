namespace AdventOfCode2022.Day.Four;

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