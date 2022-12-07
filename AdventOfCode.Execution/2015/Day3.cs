//using AdventOfCode2022.Day.X;

using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.Execution._2015;

public class Day3 : AdventOfCodeExecutionBase
{
    public Day3(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@">", 2)]
    [InlineData(@"^>v<", 4)]
    [InlineData(@"^v^v^v^v^v", 2)]
    public void Part1_Validation(string input, int expectedValue)
    {
        List<Address> addresses = new() { new Address { Count = 1 } };
        var (x, y) = (0, 0);
        foreach (var c in input)
        {
            switch (c)
            {
                case '^': x--; break;
                case 'v': x++; break;
                case '<': y--; break;
                case '>': y++; break;
            }

            var a = addresses.SingleOrDefault(i => i.X == x && i.Y == y);
            if (a is null)
            {
                a = new Address { X= x, Y = y };
                addresses.Add(a);
            }
            a.Count++;
        }

        addresses.Where(x => x.Count >= 1).Count().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"^v", 3)]
    [InlineData(@"^>v<", 3)]
    [InlineData(@"^v^v^v^v^v", 11)]
    public void Part2_Validation(string input, int expectedValue)
    {
        List<Address> addresses = new() { new Address { Count = 2 } };
        
        Santa santa = new();
        Santa roboSanta = new();
        Santa currentSanta = santa;
        foreach (var c in input)
        {
            switch (c)
            {
                case '^': currentSanta.X--; break;
                case 'v': currentSanta.X++; break;
                case '<': currentSanta.Y--; break;
                case '>': currentSanta.Y++; break;
            }

            var a = addresses.SingleOrDefault(i => i.X == currentSanta.X && i.Y == currentSanta.Y);
            if (a is null)
            {
                a = new Address { X = currentSanta.X, Y = currentSanta.Y };
                addresses.Add(a);
            }
            a.Count++;

            currentSanta = currentSanta == santa ? roboSanta : santa;            
        }

        addresses.Where(x => x.Count >= 1).Count().Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            List<Address> addresses = new() { new Address { Count = 1 } };
            var (x, y) = (0, 0);
            foreach (var c in lines[0])
            {
                switch (c)
                {
                    case '^': x--; break;
                    case 'v': x++; break;
                    case '<': y--; break;
                    case '>': y++; break;
                }

                var a = addresses.SingleOrDefault(i => i.X == x && i.Y == y);
                if (a is null)
                {
                    a = new Address { X = x, Y = y };
                    addresses.Add(a);
                }
                a.Count++;
            }
            return addresses.Where(x => x.Count >= 1).Count();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            List<Address> addresses = new() { new Address { Count = 2 } };

            Santa santa = new();
            Santa roboSanta = new();
            Santa currentSanta = santa;
            foreach (var c in lines[0])
            {
                switch (c)
                {
                    case '^': currentSanta.X--; break;
                    case 'v': currentSanta.X++; break;
                    case '<': currentSanta.Y--; break;
                    case '>': currentSanta.Y++; break;
                }

                var a = addresses.SingleOrDefault(i => i.X == currentSanta.X && i.Y == currentSanta.Y);
                if (a is null)
                {
                    a = new Address { X = currentSanta.X, Y = currentSanta.Y };
                    addresses.Add(a);
                }
                a.Count++;

                currentSanta = currentSanta == santa ? roboSanta : santa;
            }

            return addresses.Where(x => x.Count >= 1).Count();
        });
    }
}

public class Santa
{
    public int X { get; set; }
    public int Y { get; set; }
}

[DebuggerDisplay("{Count} ({X},{Y})")]
class Address
{
    public int Count { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}