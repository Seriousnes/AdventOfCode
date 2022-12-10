namespace AdventOfCode.Execution._2022;

public class Day3 : AdventOfCodeExecutionBase
{
    public Day3(ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData(157, "vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg", "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
    public void Part1_Validation(int expectedSum, params string[] rucksacks)
    {
        rucksacks.ToList()
            .Select(Rucksack.GetDuplicates)
            .Select(Rucksack.GetPriority)
            .Sum()
            .Should().Be(expectedSum);
    }

    [Theory]
    [InlineData('r', 18, "vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg")]
    [InlineData('Z', 52, "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
    public void Part2_Validation(char Badge, int Priority, params string[] rucksacks)
    {
        var badge = Rucksack.GetGroupIdentifier(rucksacks);
        var sum = Rucksack.GetPriority(new[] { badge });

        badge.Should().Be(Badge);
        sum.Should().Be(Priority);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve((lines) =>
        {
            int sum = 0;
            foreach (var line in lines)
                sum += Rucksack.CalculatePriority(line);
            return sum;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve((lines) =>
        {
            var groups = new List<char>();
            var currentGroup = new List<string>();
            foreach (var line in lines)
            {
                currentGroup ??= new List<string>();
                currentGroup.Add(line);

                if (currentGroup.Count == 3)
                {
                    groups.Add(Rucksack.GetGroupIdentifier(currentGroup));
                    currentGroup = null;
                }
            }
            return groups.Select(x => Rucksack.GetPriority(new[] { x })).Sum();
        });
    }
}

public static class Rucksack
{
    public static IEnumerable<char> GetDuplicates(string content)
    {
        var mid = content.Length / 2;
        var (compartment1, compartment2) = (content[..mid], content[mid..]);
        return compartment1
            .ToList()
            .Intersect(compartment2);
    }

    public static int ItemPriority(char c) => char.IsUpper(c) ? c - 38 : c - 96;

    public static int GetPriority(IEnumerable<char> items)
    {
        return items
            .Select(ItemPriority)
            .Sum();
    }

    public static int CalculatePriority(string content)
    {
        return GetPriority(GetDuplicates(content));
    }

    public static char GetGroupIdentifier(IList<string> rucksacks)
    {
        if (rucksacks.Count != 3)
            throw new ArgumentOutOfRangeException(nameof(rucksacks), "rucksacks must contain exactly 3 items");
        var h = new HashSet<char>(rucksacks[0]);
        h.IntersectWith(rucksacks[1]);
        h.IntersectWith(rucksacks[2]);
        return h.First();
    }
}