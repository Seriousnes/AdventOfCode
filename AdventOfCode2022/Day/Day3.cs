using System.Security.Cryptography;

namespace AdventOfCode2022.Day.Three;

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