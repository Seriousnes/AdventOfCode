namespace AdventOfCode2022.Day.One;

[DebuggerDisplay("{Calories} from {Food.Count} food items")]
public class Elf
{
    public List<int> Food { get; set; } = new();
    public int Calories => Food.Sum();
}