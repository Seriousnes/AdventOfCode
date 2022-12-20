using Newtonsoft.Json.Linq;
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
[1,[2,[3,[4,[5,6,0]]]],8,9]", 13)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var lines = input.ToLines();

        lines.ToPacketGroups().IndiceSum().Should().Be(expectedValue);
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
[1,[2,[3,[4,[5,6,0]]]],8,9]", 140)]
    public void Part2_Validation(string input, int expectedValue)
    {
        (Packet First, Packet Second) decoderPackets = (new Packet("[[2]]"), new Packet("[[6]]"));

        var packets = input.ToLines().ToPackets().ToList();

        packets.AddRange(new[] { decoderPackets.First, decoderPackets.Second });

        packets = packets.Order().ToList();

        var firstIndex = packets.IndexOf(decoderPackets.First) + 1;
        var secondIndex = packets.IndexOf(decoderPackets.Second) + 1;

        (firstIndex * secondIndex).Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            return lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToPacketGroups().IndiceSum();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            (Packet First, Packet Second) decoderPackets = (new Packet("[[2]]"), new Packet("[[6]]"));

            var packets = lines.ToPackets().ToList();

            packets.AddRange(new[] { decoderPackets.First, decoderPackets.Second });

            packets = packets.Order().ToList();

            var firstIndex = packets.IndexOf(decoderPackets.First) + 1;
            var secondIndex = packets.IndexOf(decoderPackets.Second) + 1;

            return (firstIndex * secondIndex);
        });
    }

    [Theory]
    [InlineData("[1,1,3,1,1]", "[1,1,5,1,1]", true)]
    [InlineData("[[1],[2,3,4]]", "[[1],4]", true)]
    [InlineData("[9]", "[[8,7,6]]", false)]
    [InlineData("[[4,4],4,4]", "[[4,4],4,4,4]", true)]
    [InlineData("[7,7,7,7]", "[7,7,7]", false)]
    [InlineData("[]", "[3]", true)]
    [InlineData("[[[]]]", "[[]]", false)]
    [InlineData("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", false)]
    public void PacketGroupValidation(string left, string right, bool expectedValue)
    {
        var pg = new PacketGroup(left, right);
        pg.IsInOrder().Should().Be(expectedValue);
    }
}

public interface ISignal : IComparable<ISignal>
{
    OrderResult IsSmallerThan(ISignal other);
    string ToString();
}

[DebuggerDisplay("{ToString()}")]
public readonly record struct Signal: ISignal
{
    public Signal(int value)
    {
        this.Value = value;
    }

    public Signal(string value) : this(int.Parse(value)) { }

    public int Value { get; }

    public int CompareTo(ISignal other)
    {
        return IsSmallerThan(other) switch
        {
            OrderResult.IsSmaller => -1,
            OrderResult.IsEqual => 0,
            OrderResult.IsLarger => 1
        };
    }

    public OrderResult IsSmallerThan(ISignal other)
    {
        if (other is Packet packet)
        {
            var thisP = new Packet { this };
            return thisP.IsSmallerThan(other);
        }

        return (Value - ((Signal)other).Value) switch
        {
            > 0 => OrderResult.IsLarger,
            < 0 => OrderResult.IsSmaller,
            _ => OrderResult.IsEqual
        };
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

[DebuggerDisplay("{ToString()}")]
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

    public Packet(string jsonString) : this (JsonSerializer.Deserialize<JsonArray>(jsonString)!)
    {
    }

    public int CompareTo(ISignal? other)
    {
        return IsSmallerThan(other) switch
        {
            OrderResult.IsSmaller => -1,
            OrderResult.IsEqual => 0,
            OrderResult.IsLarger => 1
        };
    }

    public OrderResult IsSmallerThan(ISignal other)
    {
        var otherPacket = other is Packet ? (Packet)other : new Packet { other };
        for (int i = 0; i < Math.Max(Count, otherPacket.Count); i++)
        {
            if (i >= Count)
                return OrderResult.IsSmaller;
            if (i >= otherPacket.Count)
                return OrderResult.IsLarger;

            var result = this[i].IsSmallerThan(otherPacket[i]);
            if (result != OrderResult.IsEqual)
                return result;
        }
        return OrderResult.IsEqual;
    }

    public override string ToString()
    {
        return $"[{string.Join(',', this)}]";   
    }
}

public class PacketGroup
{
    public PacketGroup() { }

    public PacketGroup(IEnumerable<Packet> packets)
    {
        Left = packets.First();
        Right = packets.Last();
    }

    public PacketGroup(string left, string right)
    {
        Left = new Packet(left);
        Right = new Packet(right);
    }

    public Packet Left { get; set; }
    public Packet Right { get; set; }

    public bool IsInOrder()
    {
        return Left.IsSmallerThan(Right) == OrderResult.IsSmaller;
    }
}

public enum OrderResult
{
    IsSmaller,
    IsEqual,
    IsLarger
}

public static class Day13Extensions
{
    public static int IndiceSum(this IEnumerable<PacketGroup> packets) => packets
        .Select((pg, index) => new { IsInOrder = pg.IsInOrder(), Index = index + 1 })
        .Where(x => x.IsInOrder)
        .Sum(x => x.Index);

    public static IEnumerable<PacketGroup> ToPacketGroups(this IEnumerable<string> lines) => lines.ToPackets()
        .Select((packet, index) => new { packet, index })
        .GroupBy(x => x.index / 2, x => x.packet)
        .Cast<IEnumerable<Packet>>()
        .Select(x => new PacketGroup { Left = x.First(), Right = x.Last() });

    public static IEnumerable<Packet> ToPackets(this IEnumerable<string> lines) => lines.Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => new Packet(x));
}