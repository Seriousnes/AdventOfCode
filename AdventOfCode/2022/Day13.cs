using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode.Execution._2022;

public class Day13 : AdventOfCodeExecutionBase
{
    public Day13(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]", null)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var lines = input.ToLines().Where(s => !string.IsNullOrWhiteSpace(s));

        var signals = new List<JsonArray>();
        foreach (var line in lines)
        {
            signals.Add(JsonSerializer.Deserialize<JsonArray>(line));
        }

        var pg = signals.Select((s, i) => new { Index = i, Signal = s })
            .GroupBy(x => x.Index / 2, x => new Packet(x.Signal))
            .Cast<IEnumerable<Packet>>()
            .ToList()
            .Select(g => new PacketGroup(g));

    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, int expectedValue)
    {
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            return default;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            return default;
        });
    }
}

public interface ISignal
{
    OrderResult IsSmallerThan(ISignal other);
}

[DebuggerDisplay("{Value}")]
public record struct Signal: ISignal
{
    public Signal(int value)
    {
        this.Value = value;
    }

    public Signal(string value) : this(int.Parse(value)) { }

    public int Value { get; }

    public OrderResult IsSmallerThan(ISignal other)
    {
        if (other is Packet packet)
        {
            var thisP = new Packet { this };
            return thisP.IsSmallerThan(other);
        }

        return (((Signal)other).Value - Value) switch
        {
            > 0 => OrderResult.IsLarger,
            < 0 => OrderResult.IsSmaller,
            _ => OrderResult.IsEqual
        };
    }
}

public class Packet : List<ISignal>, ISignal
{
    public Packet() : base()
    {
    }

    public Packet(JsonArray jsonArray)
    {
        foreach (var node in jsonArray)
        {
            Add(node switch
            {
                var _ when node is JsonArray subArray => new Packet(subArray),
                _ => new Signal(node.GetValue<int>())
            });
        }
    }

    public OrderResult IsSmallerThan(ISignal other)
    {
        if (other is Packet otherPacket)
        {
            for (int i = 0; i < Math.Max(Count, otherPacket.Count); i++)
            {
                if (i >= (Count - 1))
                    return OrderResult.IsLarger;
                if (i >= (otherPacket.Count - 1))
                    return OrderResult.IsSmaller;

                var result = this[i].IsSmallerThan(otherPacket[i]);
                if (result != OrderResult.IsEqual)
                    return result;
            }
            return OrderResult.IsEqual;
        }
        else
        {
            var signalAsPacket = new Packet { (Signal)other };
            return IsSmallerThan(signalAsPacket);
        }
    }
}

public class PacketGroup
{
    public PacketGroup(IEnumerable<Packet> packets)
    {
        Left = packets.First();
        Right = packets.Last();
    }

    public Packet Left { get; set; }
    public Packet Right { get; set; }

    public int GetIndiceSum()
    {
        return 0;// Left.IsSmallerThan(Right) == OrderResult.IsSmaller;
    }
}

public enum OrderResult
{
    IsSmaller,
    IsEqual,
    IsLarger
}