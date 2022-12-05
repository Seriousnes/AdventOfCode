using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day.Five;

[DebuggerDisplay("{ToStack()}")]
public class CrateStack : Stack<string>
{
    public CrateStack() : base() { }

    public CrateStack(IEnumerable<string> values) : base(values) { }

    public string ToStack()
    {
        return string.Join(" ", this.Select(x => $"[{x}]").Reverse());
    }
}

public class CargoShip
{
    public List<CrateStack> Crates { get; set; } = new();
    public CrateStack this[int Index] => Crates[Index];
    public void ProcessInstruction(Instruction instruction)
    {
        (var source, var dest) = (this[instruction.FromIndex - 1], this[instruction.ToIndex - 1]);
        for (int i = 1; i <= instruction.Count; i++)
        {
            dest.Push(source.Pop());
        }
    }

    public void ProcessInstructionFILO(Instruction instruction)
    {
        try
        {
            (var source, var dest) = (this[instruction.FromIndex - 1], this[instruction.ToIndex - 1]);
            var moving = source.Take(instruction.Count);
            Crates[instruction.FromIndex - 1] = new CrateStack(source.Skip(instruction.Count).Reverse());
            moving.Reverse().ToList().ForEach(dest.Push);
        }
        catch (Exception e)
        {            
            throw new Exception($"Invalid instruction {instruction}", e);
        }
    }

    public string GetMessage()
    {
        var sb = new StringBuilder();
        foreach(var c in Crates.Where(x => x.Any()))
        {
            sb.Append(c.Pop());
        }
        return sb.ToString();
    }
}

public class Instruction
{
    private static Regex _r = new Regex(@"move (\d+) from (\d+) to (\d+)", RegexOptions.Compiled);
    public Instruction(string input)
    {
        var parts = _r.Matches(input)[0].Groups.Values.Skip(1).Select(x => int.Parse(x.Value)).ToList();
        (Count, FromIndex, ToIndex) = (parts[0], parts[1], parts[2]);
    }
        
    public int Count { get; set; }
    public int FromIndex { get; set; }
    public int ToIndex { get; set; }
    public override string ToString() => $"move {Count} from {FromIndex} to {ToIndex}";
}