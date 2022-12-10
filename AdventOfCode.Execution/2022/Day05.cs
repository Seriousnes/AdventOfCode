using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Execution._2022;

public class Day5 : AdventOfCodeExecutionBase
{
    public Day5(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"[Z]     [P] [Q] [B]     [S] [W] [P]", null)]
    public void Part1_Validation(string input, object expectedValue)
    {
        var cs = new CargoShip();
        var r = new Regex(@"(?:\[([A-Z])\]|\s{3})\s?");
        var crates = r.Matches(input)
            .Select((x, i) => new { Cargo = x.Groups[1].Value, Index = i })
            .ToList();

        for (int i = 0; i < crates.Count; i++)
            cs.Crates.Add(new CrateStack());

        crates.ForEach(a =>
            {                
                if (!string.IsNullOrWhiteSpace(a.Cargo))
                {                    
                    cs[a.Index].Push(a.Cargo);
                }
            });

        output.WriteLine(cs.GetMessage());
    }

    [Theory]
    [InlineData(@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2", "MCD")]
    public void Part2_Validation(string input, object expectedValue)
    {
        var lines = input.Split(Environment.NewLine);
        var r = new Regex(@"(?:\[([A-Z])\]|\s{3})\s?", RegexOptions.Compiled);
        var cs = new CargoShip();
        for (int i = 0; i <= 2; i++)
            cs.Crates.Add(new CrateStack());
        for (int i = 2; i >= 0; i--)
        {
            r.Matches(lines[i])
                .Select((x, i) => new { Cargo = x.Groups[1].Value, Index = i })
                .ToList()
                .ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.Cargo))
                    {
                        cs[x.Index].Push(x.Cargo);
                    }
                });
        }

        foreach (var line in lines.Skip(5))
        {
            cs.ProcessInstructionFILO(new Instruction(line));
        }

        output.WriteLine(cs.GetMessage());
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var r = new Regex(@"(?:\[([A-Z])\]|\s{3})\s?", RegexOptions.Compiled);

            var cs = new CargoShip();
            for (int i = 1; i <= 9; i++)
                cs.Crates.Add(new CrateStack());
            for (int i = 8; i >= 0; i--)
            {
                r.Matches(lines[i])
                    .Select((x, i) => new { Cargo = x.Groups[1].Value, Index = i })
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.Cargo))
                        {
                            cs[x.Index].Push(x.Cargo);
                        }
                    });
            }

            foreach (var line in lines.Skip(10))
            {
                cs.ProcessInstruction(new Instruction(line));
            }

            return cs.GetMessage();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve((lines) =>
        {
            var r = new Regex(@"(?:\[([A-Z])\]|\s{3})\s?", RegexOptions.Compiled);

            var cs = new CargoShip();
            for (int i = 1; i <= 9; i++)
                cs.Crates.Add(new CrateStack());
            for (int i = 8; i >= 0; i--)
            {
                r.Matches(lines[i])
                    .Select((x, i) => new { Cargo = x.Groups[1].Value, Index = i })
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.Cargo))
                        {
                            cs[x.Index].Push(x.Cargo);
                        }
                    });
            }

            foreach (var line in lines.Skip(10))
            {
                cs.ProcessInstructionFILO(new Instruction(line));
            }

            return cs.GetMessage();
        });
    }
}

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
        (var source, var dest) = (this[instruction.FromIndex - 1], this[instruction.ToIndex - 1]);
        var intermediateList = new CrateStack();
        for (int i = 1; i <= instruction.Count; i++)
            intermediateList.Push(source.Pop());
        for (int i = 1; i <= instruction.Count; i++)
            dest.Push(intermediateList.Pop());
    }

    public string GetMessage()
    {
        var sb = new StringBuilder();
        foreach (var c in Crates.Where(x => x.Any()))
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