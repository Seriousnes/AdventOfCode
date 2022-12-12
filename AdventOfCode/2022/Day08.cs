//using AdventOfCode2022.Day.Eight;

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode.Execution._2022;

public class Day8 : AdventOfCodeExecutionBase
{
    public Day8(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"30373
25512
65332
33549
35390", 21)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var f = GetForest(input.Split(Environment.NewLine));
        for (int i = 0; i < f.GetLength(0); i++)
        {
            var s = "";
            for (int j = 0; j < f.GetLength(1); j++)
                s += $"{f[j, i]}";
            output.WriteLine(s);
        }

        output.WriteLine("");



        int visibleTrees = 0;
        for (int i = 0; i < f.GetLength(0); i++)
        {
            var s = "";
            for (int j = 0; j < f.GetLength(1); j++)
            {
                if (TreeIsVisible(j, i, f))
                { 
                    visibleTrees++;
                    s += "T";
                }
                else
                {
                    s += "F";
                }                
            }
            output.WriteLine(s);
        }                
        
        visibleTrees.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"30373
25512
65332
33549
35390", 2, 1, 4)]
    [InlineData(@"30373
25512
65332
33549
35390", 2, 3, 8)]
    public void Part2_Validation(string input, int x, int y, int expectedValue)
    {
        var f = GetForest(input.Split(Environment.NewLine));

        output.WriteLine(f.ToForest((x, y)));

        GetScenicScore(x, y, f).Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var f = GetForest(lines);

            int visibleTrees = 0;
            for (int i = 0; i < f.GetLength(0); i++)
                for (int j = 0; j < f.GetLength(1); j++)
                    if (TreeIsVisible(j, i, f))
                        visibleTrees++;
            return visibleTrees;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var f = lines.To2DArray(l => l.ToArray(), c => int.Parse(c.ToString()));
            var maxScore = 0;

            for (int y = 0; y < f.GetLength(1); y++)
            {
                for (int x = 0; x < f.GetLength(0); x++)
                {
                    maxScore = Math.Max(maxScore, GetScenicScore(x, y, f));
                }
            }
            return maxScore;
        });
    }

    private int[,] GetForest(IEnumerable<string> input)
    {
        var width = input.First().Length;
        var height = input.Count();

        int[,] forest = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            var row = input.ElementAt(y);
            for (int x = 0; x < width; x++)
            {
                forest[x, y] = int.Parse(row[x].ToString());
            }
        }
        return forest;
    }

    
    private bool TreeIsVisible(int x, int y, int[,] forest)
    {
        var width = forest.GetLength(0);
        var height = forest.GetLength(1);

        var dimensionX = Enumerable.Range(0, width).Select(i => forest[i, y]).ToList();
        var dimensionY = Enumerable.Range(0, height).Select(i => forest[x, i]).ToList();

        var treeHeight = forest[x, y];
        var (fromLeft, fromRight, fromTop, fromBottom) =
            (
                dimensionX.Take(x).All(t => t < treeHeight),
                dimensionX.Skip(x + 1).All(t => t < treeHeight),
                dimensionY.Take(y).All(t => t < treeHeight),
                dimensionY.Skip(y + 1).All(t => t < treeHeight)
            );

        return fromLeft || fromRight || fromTop || fromBottom;            
    }    


    private int GetScenicScore(int x, int y, int[,] forest)
    {
        var treeHeight = forest[x, y];
        var width = forest.GetLength(0);
        var height = forest.GetLength(1);

        var dimensionX = Enumerable.Range(0, width).Select(i => forest[i, y]).ToArray();
        var dimensionY = Enumerable.Range(0, height).Select(i => forest[x, i]).ToArray();

        var getDimentionDistances = (int[] array, int initialIndex, Action<int> lowerBound, Action<int> upperBound) =>
        {
            int v = 0;
            for (int i = initialIndex - 1; i > 0; i--)
                if (array[i] >= treeHeight)
                {
                    v = i;
                    break;
                }
            lowerBound(v);

            v = array.Length - 1;
            for (int i = initialIndex + 1; i < array.Length; i++)
                if (array[i] >= treeHeight)
                {
                    v = i;
                    break;
                }
                    
            upperBound(v);
        };

        var (l, r, t, b) = (0, 0, 0, 0);
        getDimentionDistances(dimensionX, x, i => l = x - i, i => r = i - x);
        getDimentionDistances(dimensionY, y, i => t = y - i, i => b = i - y);
        return l * r * t * b;
            
    }
}

public static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<int>> SplitArrayAt(this IEnumerable<int> array, int position)
    {
        yield return array.Take(position).ToList();
        yield return array.Skip(position + 1).ToList();
    }

    public static string ToForest(this int[,] forest, (int X, int Y) select)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < forest.GetLength(0); i++)
        {
            for (int j = 0; j < forest.GetLength(1); j++)
            {
                if (i == select.Y && j == select.X)
                    sb.Append($"[{forest[j, i]}]");
                else
                    sb.Append($" {forest[j, i]} ");
            }
                
            sb.Append('\n');
        }
        return sb.ToString();
    }
}