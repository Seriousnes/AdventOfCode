namespace AdventOfCode.Execution._2022;

public class Day9 : AdventOfCodeExecutionBase
{
    public Day9(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2", 13)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var r = new Rope(1);
        r.FollowInstructions(input.Split(Environment.NewLine));
        r.UniqueTailPositions().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20", 36)]
    public void Part2_Validation(string input, int expectedValue)
    {
        var r = new Rope(9);
        r.FollowInstructions(input.Split(Environment.NewLine));
        r.UniqueTailPositions().Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var rope = new Rope(1);
            rope.FollowInstructions(lines);
            return rope.UniqueTailPositions();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var rope = new Rope(9);
            rope.FollowInstructions(lines);
            return rope.UniqueTailPositions();
        });
    }    
}


[DebuggerDisplay("({Column}, {Row})")]
public class Position
{
    public required int Column { get; set; }
    public required int Row { get; set; }
    public Position Tail { get; set; }
    public Position EndOfRope => Tail is null ? this : Tail.EndOfRope;

    public override string ToString()
    {
        return $"({Column},{Row}){(Tail is not null ? $" -> {Tail}" : "")}";
    }

    public void Move(Direction d)
    {
        DoMove(d);
        MakeTailAdjacent();
    }

    public void DoMove(Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                Row++;
                break;
            case Direction.Down:
                Row--;
                break;
            case Direction.Left:
                Column--;
                break;
            case Direction.Right:
                Column++;
                break;
        }
    }

    public void MakeTailAdjacent()
    {
        if (Tail is null) return;

        var moveDiagonally = (int h, int t, Direction a, Direction b) =>
        {            
            if (h != t)
                Tail.DoMove(h - t > 0 ? a : b);
        };

        if (Math.Abs(Column - Tail.Column) > 1)
        {
            moveDiagonally(Row, Tail.Row, Direction.Up, Direction.Down);
            Tail.Move(Column > Tail.Column ? Direction.Right : Direction.Left);
        }

        if (Math.Abs(Row - Tail.Row) > 1)
        {
            moveDiagonally(Column, Tail.Column, Direction.Right, Direction.Left);
            Tail.Move(Row > Tail.Row ? Direction.Up : Direction.Down);
        }
    }
}

[DebuggerDisplay("({Column}, {Row})")]
public struct UniquePosition
{
    public int Column { get; set; }
    public int Row { get; set; }

    public static UniquePosition FromPosition(Position p)
    {
        return new() { Column = p.Column, Row = p.Row };
    }
}

[DebuggerDisplay("{Head.ToString()}")]
public class Rope
{
    public Rope(int numTails)
    {
        var s = Head;
        for (int i = 0; i < numTails; i++)
        {
            s.Tail = new Position { Column = 0, Row = 0 };
            s = s.Tail;
        }
    }

    public Position Head { get; set; } = new() { Column = 0, Row = 0 };

    private List<UniquePosition> allTailPositions = new List<UniquePosition>();

    public int UniqueTailPositions() => allTailPositions.Distinct().Count();

    public void FollowInstructions(string[] instructions)
    {
        allTailPositions.Add(UniquePosition.FromPosition(Head.Tail));
        foreach (var line in instructions.Select(l => l.Split(' ')))
        {
            MoveHead((Direction)line[0][0], int.Parse(line[1].ToString()));
        }
    }
    
    public void MoveHead(Direction direction, int distance)
    {
        for (int i = 0; i < distance; i++)
        {
            Head.Move(direction);
            allTailPositions.Add(UniquePosition.FromPosition(Head.EndOfRope));
        }
    }
}

public enum Direction
{
    Up = 'U',
    Down = 'D', 
    Right = 'R',
    Left = 'L'
}