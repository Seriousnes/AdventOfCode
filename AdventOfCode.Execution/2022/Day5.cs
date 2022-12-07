using AdventOfCode2022.Day.Five;
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