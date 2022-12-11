//using AdventOfCode2022.Day.X;

namespace AdventOfCode.Execution._2015;

public class Day01 : AdventOfCodeExecutionBase
{
    public Day01(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"(())", 0)]
    [InlineData(@"()()", 0)]
    [InlineData(@"(((", 3)]
    [InlineData(@"(()(()(", 3)]
    [InlineData(@"))(((((", 3)]
    [InlineData(@"())", -1)]
    [InlineData(@"))(", -1)]
    [InlineData(@")))", -3)]
    [InlineData(@")())())", -3)]
    public void Part1_Validation(string input, int expectedValue)
    {
        int floor = 0;
        for (int i = 0; i < input.Length; i++)
        {
            switch ((Direction)input[i])
            {
                case Direction.Up:
                    floor++;
                    break;
                case Direction.Down: 
                    floor--; 
                    break;
            }
        }
        floor.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, object expectedValue)
    {
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve((lines) =>
        {
            int floor = 0;
            for (int i = 0; i < lines[0].Length; i++)
            {
                switch ((Direction)lines[0][i])
                {
                    case Direction.Up:
                        floor++;
                        break;
                    case Direction.Down:
                        floor--;
                        break;
                }
            }

            return floor;
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            int floor = 0;
            for (int i = 0; i < lines[0].Length; i++)
            {
                switch ((Direction)lines[0][i])
                {
                    case Direction.Up:
                        floor++;
                        break;
                    case Direction.Down:
                        floor--;
                        break;
                }

                if (floor == -1)
                    return i + 1;
            }

            return -1;
        });
    }
}

public enum Direction
{
    Up = '(',
    Down = ')'
}