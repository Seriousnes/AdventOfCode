using AdventOfCode.Execution.Framework;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace AdventOfCode.Execution._2022;

public class Day12 : AdventOfCodeExecutionBase
{
    public Day12(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi", 31)]
    public async Task Part1_Validation(string input, int expectedValue)
    {
        var h = new HeightMap(input.ToLines());
        h.GetShortestPath().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi", 29)]
    public void Part2_Validation(string input, int expectedValue)
    {
        var h = new HeightMap(input.ToLines());
        var pathDistances = new ConcurrentBag<int>();
        Parallel.ForEach(h.GetStartingPoints(false), (p) =>
        {
            pathDistances.Add(h.GetShortestPath(p));
        });

        pathDistances.Min().Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var h = new HeightMap(lines);
            return h.GetShortestPath();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var h = new HeightMap(lines);
            var pathDistances = new ConcurrentBag<int>();
            Parallel.ForEach(h.GetStartingPoints(false), (p) =>
            {
                pathDistances.Add(h.GetShortestPath(p));
            });

            return pathDistances.Min();
        });
    }
}

public class HeightMap
{
    private char[,] _grid;
    
    public HeightMap(IEnumerable<string> lines)
    {
        _grid = lines.To2DArray(l => l.ToArray(), (elevation, x, y) =>
        {
            if (elevation == 'E')
                Goal = new GridRef { X = x, Y = y };

            return elevation;
        });
    }

    public int this[int X, int Y] => _grid[X, Y] switch
    {
        'S' => 'a',
        'E' => 'z',
        var x => x
    } - 96;

    public int this[GridRef p] => this[p.X, p.Y];
    public GridRef Goal { get; private set; }
    public int Width => _grid.GetLength(0);
    public int Height => _grid.GetLength(1);

    public int GetShortestPath(GridRef start = null)
    {
        start ??= GetStartingPoints().First();

        start.SetDistance(Goal);

        var _visitedNodes = new List<GridRef>();
        var _activeNodes = new List<GridRef>() { start };

        while (_activeNodes.Any())
        {
            var check = _activeNodes.OrderBy(x => x.CostDistance).First();
            if (check.IsAtSamePosition(Goal))
                return check.Cost;

            _visitedNodes.Add(check);
            _activeNodes.Remove(check);

            var nextNodes = check.GetPossibleMoves(this);
            foreach (var next in nextNodes.Where(x => !_visitedNodes.Any(v => v.IsAtSamePosition(x))))
            {
                if (_activeNodes.Any(x => x.IsAtSamePosition(next)))
                {
                    var existing = _activeNodes.First(x => x.IsAtSamePosition(next));
                    if (existing.CostDistance > check.CostDistance)
                    {
                        _activeNodes.Remove(existing);
                        _activeNodes.Add(next);
                    }
                }
                else
                {
                    _activeNodes.Add(next);
                }
            }
        }

        return int.MaxValue;
    }

    public IEnumerable<GridRef> GetStartingPoints(bool useInitial = true)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var height = _grid[x, y];
                if ((useInitial && height == 'S') || (!useInitial && height.In('S', 'a')))
                    yield return new GridRef { X = x, Y = y };
            }
        }
    }
}

[DebuggerDisplay("{X},{Y}")]
public class GridRef
{
    public GridRef() { }

    public GridRef(GridRef current, GridDirection direction, GridRef target)
    {
        Parent = current;

        X = direction switch
        {
            GridDirection.Left => current.X - 1,
            GridDirection.Right => current.X + 1,
            _ => current.X
        };

        Y = direction switch
        {
            GridDirection.Up => current.Y - 1,
            GridDirection.Down => current.Y + 1,
            _ => current.Y
        };

        Cost = current.Cost + 1;
        SetDistance(target);
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public GridRef Parent { get; set; }

    public void SetDistance(int x, int y)
    {
        Distance = Math.Abs(x - X) + Math.Abs(y - Y);
    }

    internal void SetDistance(GridRef end)
    {
        SetDistance(end.X, end.Y);
    }

    public bool IsAtSamePosition(GridRef rhs)
    {
        return X == rhs.X && Y == rhs.Y;
    }
}

public enum GridDirection
{
    Up = '^',
    Down = 'V',
    Right = '>',
    Left = '<'
}

public static class Day12Extensions
{
    public static IEnumerable<GridRef> GetPossibleMoves(this GridRef position, HeightMap grid)
    {
        return new List<GridRef>(Enum.GetValues<GridDirection>()
            .Select(x => new GridRef(position, x, grid.Goal)))
            .Where(x => x.IsInGrid(grid) && grid[x] - grid[position] <= 1);
    }

    public static bool IsInGrid(this GridRef position, HeightMap grid) => 
        position.X.IsBetween(0, grid.Width, upperBoundInclusive: false) && 
        position.Y.IsBetween(0, grid.Height, upperBoundInclusive: false);
}