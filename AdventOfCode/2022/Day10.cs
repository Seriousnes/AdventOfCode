using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode.Execution._2022;

public class Day10 : AdventOfCodeExecutionBase
{
    public Day10(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop", 13140)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var lines = input.Split(Environment.NewLine);
        var cpu = new CPU();
        foreach (var line in lines.Select(x => x.Split(' ')))
        {
            var instruction = line[0].ToInstruction();
            int? value = null;
            if (instruction == CpuInstruction.addx)
                value = int.Parse(line[1]);
            cpu.Input(instruction, value);
        }

        var signalStrengths = new[] { 20, 60, 100, 140, 180, 220 }.Select(cpu.SignalStrength);
        
        signalStrengths.Sum().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop", @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....")]
    public void Part2_Validation(string input, string expectedValue)
    {
        var lines = input.Split(Environment.NewLine);
        var cpu = new CPU();
        foreach (var line in lines.Select(x => x.Split(' ')))
        {
            var instruction = line[0].ToInstruction();
            int? value = null;
            if (instruction == CpuInstruction.addx)
                value = int.Parse(line[1]);
            cpu.Input(instruction, value);
        }

        var sb = new StringBuilder();
        for (int i = 0; i < 220; i++)
        {
            var registerX = cpu.RegisterValue(i + 1);
            var sprite = new Range(Math.Max(registerX - 1, 0), Math.Min(registerX + 1, 40));

            var horizontalPosition = i % 40;

            bool isDrawing = horizontalPosition >= sprite.Start.Value && horizontalPosition <= sprite.End.Value;

            sb.Append(isDrawing ? "#" : " ");
            if (i > 0 && (i + 1) % 40 == 0)
            {
                sb.Append(Environment.NewLine);
            }
        }

        sb.ToString().Split(Environment.NewLine).ToList().ForEach(x => output.WriteLine(x));
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var cpu = new CPU();
            foreach (var line in lines.Select(x => x.Split(' ')))
            {
                var instruction = line[0].ToInstruction();
                int? value = null;
                if (instruction == CpuInstruction.addx)
                    value = int.Parse(line[1]);
                cpu.Input(instruction, value);
            }

            var signalStrengths = new[] { 20, 60, 100, 140, 180, 220 }.Select(cpu.SignalStrength);

            return signalStrengths.Sum();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var cpu = new CPU();
            foreach (var line in lines.Select(x => x.Split(' ')))
            {
                var instruction = line[0].ToInstruction();
                int? value = null;
                if (instruction == CpuInstruction.addx)
                    value = int.Parse(line[1]);
                cpu.Input(instruction, value);
            }

            var sb = new StringBuilder();
            for (int i = 0; i < 240; i++)
            {
                var registerX = cpu.RegisterValue(i + 1);
                var sprite = new Range(Math.Max(registerX - 1, 0), Math.Min(registerX + 1, 40));

                var horizontalPosition = i % 40;

                bool isDrawing = horizontalPosition >= sprite.Start.Value && horizontalPosition <= sprite.End.Value;

                sb.Append(isDrawing ? "#" : " ");
                if (i > 0 && (i + 1) % 40 == 0)
                {
                    sb.Append(Environment.NewLine);
                }
            }

            output.WriteLine(new string('=', 40));
            foreach (var line in sb.ToString().Split(Environment.NewLine))
                output.WriteLine(line);
            output.WriteLine(new string('=', 40));

            return null;
        });
    }
}

public class CPU
{
    private Dictionary<int, int> registerHistory = new();
    public int Cycle { get; internal set; }
    public int Register { get; internal set; } = 1;

    public int SignalStrength(int atCycle)
    {
        return atCycle * RegisterValue(atCycle);
    }

    public int RegisterValue(int atCycle)
    {
        return registerHistory.Where(x => x.Key < atCycle).OrderByDescending(x => x.Key).Select(x => x.Value).FirstOrDefault(1);
    }

    public void Input(CpuInstruction instruction, int? value = null)
    {
        switch (instruction)
        {
            case CpuInstruction.addx:
                Cycle += 2;
                Register += value.GetValueOrDefault();
                break;
            case CpuInstruction.noop:
                Cycle++;
                break;
        }
        registerHistory.Add(Cycle, Register);
    }
}

public enum CpuInstruction
{
    addx,
    noop,
}

public static class Day10Extensions
{
    public static CpuInstruction ToInstruction(this string value)
    {
        return Enum.GetValues<CpuInstruction>().Single(x => x.ToString() == value);
    }

    public static IEnumerable<string> ToLines(this string value, int size)
    {
        return Enumerable.Range(0, value.Length / size).Select(i => value.Substring(i * size, size));
    }
}