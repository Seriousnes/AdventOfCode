//using AdventOfCode2022.Day.Nine;

using AdventOfCode2022.Day.Two;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        var r = new Rope();
        r.FollowInstructions(input.Split(Environment.NewLine));
        r.UniqueTailPositions().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, object expectedValue)
    {        
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var rope = new Rope();
            rope.FollowInstructions(lines);
            return rope.UniqueTailPositions();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            return default(object);
        });
    }    
}


[DebuggerDisplay("({Column}, {Row})")]
public class Position
{
    public required int Column { get; set; }
    public required int Row { get; set; }

    public void Move(Direction d)
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

    public void MakeAdjacentToPosition(Position p)
    {
        //var d = Math.Sqrt(Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2));

        var moveDiagonally = (int h, int t, Direction a, Direction b) =>
        {            
            if (h != t)
                Move(h - t > 0 ? a : b);
        };

        if (Math.Abs(p.Column - Column) > 1)
        {
            Move(p.Column > Column ? Direction.Right : Direction.Left);
            moveDiagonally(p.Row, Row, Direction.Up, Direction.Down);
        }

        if (Math.Abs(p.Row - Row) > 1)
        {
            Move(p.Row > Row ? Direction.Up : Direction.Down);
            moveDiagonally(p.Column, Column, Direction.Right, Direction.Left);
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

public class Rope
{
    public Position Head { get; set; } = new() { Column = 0, Row = 0 };
    public Position Tail { get; set; } = new() { Column = 0, Row = 0 };

    private List<UniquePosition> allTailPositions = new List<UniquePosition>();

    public int UniqueTailPositions() => allTailPositions.Distinct().Count();

    public void FollowInstructions(string[] instructions)
    {
        allTailPositions.Add(UniquePosition.FromPosition(Tail));
        foreach (var line in instructions.Select(l => l.Split(' ').Select(c => c[0]).ToArray()))
        {
            MoveHead((Direction)line[0], int.Parse(line[1].ToString()));
        }
    }
    
    public void MoveHead(Direction direction, int distance)
    {
        for (int i = 0; i < distance; i++)
        {
            Head.Move(direction);
            Tail.MakeAdjacentToPosition(Head);
            allTailPositions.Add(UniquePosition.FromPosition(Tail));
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