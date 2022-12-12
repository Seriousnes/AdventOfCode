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
        var h = new HeightMap(input.ToLines().ToList());
        var p = await h.Walk(h.Start, new List<GridRef>());
        h.GetShortestPath(p).Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, int expectedValue)
    {
    }

    [Fact]
    public async void Part1_Execution()
    {
        await SolveAsync<object>(async (lines) =>
        {
            var h = new HeightMap(lines);
            var p = await h.Walk(h.Start, new List<GridRef>());
            return h.GetShortestPath(p);
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

public class HeightMap
{
    private int[,] _grid;

    public HeightMap(IEnumerable<string> lines)
    {
        _grid = lines.To2DArray(l => l.ToArray(), (elevation, x, y) =>
        {
            if (elevation == 'S')
                Start = new GridRef(x, y);
            if (elevation == 'E')
                End = new GridRef(x, y);

            return elevation switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => elevation
            } - 96;
        });
    }

    public int this[int X, int Y] => _grid[X, Y];
    public int this[GridRef p] => _grid[p.X, p.Y];
    public GridRef Start { get; private set; }
    public  GridRef End { get; private set; }
    public int[,] Grid => _grid;
    public int Width => _grid.GetLength(1);
    public int Height => _grid.GetLength(0);

    public int GetShortestPath(List<List<GridRef>> paths)
    {
        return paths.Where(x => x.Last() == End).OrderBy(x => x.Count).Select(x => x.Count).FirstOrDefault();
    }

    public async Task<List<List<GridRef>>> Walk(GridRef startingPoint, List<GridRef> path = null)
    {
        path ??= new List<GridRef>();

        // from this position, get options and walk those
        var nextMoves = startingPoint.GetPossibleMoves(this);

        // found the end!!
        if (nextMoves.Contains(End))
        {
            path.Add(End);
            return new List<List<GridRef>>(new[] { path });
        }
        // deadend path
        else if (nextMoves.Count() == 0)
        {
            new List<List<GridRef>>(new[] { nextMoves.ToList() });
        }            

        // don't move to an already moved to position
        nextMoves = nextMoves.Where(x => !path.Contains(x));

        List<List<GridRef>> walkedPaths = new();
        
        await Parallel.ForEachAsync(nextMoves, async (m, _) =>
        {
            var moveList = new List<GridRef>(path) { m };            
            var walkResult = await Walk(m, moveList);
            walkedPaths.AddRange(walkResult.Where(x => x.Count > 0));
        });

        return walkedPaths;
    }    
}

[DebuggerDisplay("{X},{Y}")]
public struct GridRef
{
    public GridRef(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }

    public static bool operator ==(GridRef lhs, GridRef rhs)
    {
        return lhs.X == rhs.X && lhs.Y == rhs.Y;
    }

    public static bool operator !=(GridRef lhs, GridRef rhs)
    {
        return lhs.X != rhs.X || lhs.Y != rhs.Y;
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
        GridRef p;

        p = new GridRef(position.X, position.Y - 1);
        if (p.IsInGrid(grid) && Math.Abs(grid[p] - grid[position]) <= 1)
            yield return p;

        p = new GridRef(position.X, position.Y + 1);
        if (p.IsInGrid(grid) && Math.Abs(grid[p] - grid[position]) <= 1)
            yield return p;

        p = new GridRef(position.X - 1, position.Y);
        if (p.IsInGrid(grid) && Math.Abs(grid[p] - grid[position]) <= 1)
            yield return p;

        p = new GridRef(position.X + 1, position.Y);
        if (p.IsInGrid(grid) && Math.Abs(grid[p] - grid[position]) <= 1)
            yield return p;
    }

    public static bool IsInGrid(this GridRef position, HeightMap grid) => position.X < grid.Height && position.Y < grid.Width && position.X >= 0 && position.Y >= 0;
}