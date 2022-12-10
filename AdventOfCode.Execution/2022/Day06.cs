namespace AdventOfCode.Execution._2022;

public class Day6 : AdventOfCodeExecutionBase
{
    public Day6(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    public void Part1_Validation(string input, int expectedValue)
    {
        var dataStream = new Subroutine { DataStreamBuffer = input };
        dataStream.GetStartMarker(4).Should().Be(expectedValue);
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
            var s = new Subroutine { DataStreamBuffer = lines[0] };
            return s.GetStartMarker(4);
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var s = new Subroutine { DataStreamBuffer = lines[0] };
            return s.GetStartMarker(14);
        });
    }
}

public class Subroutine
{
    public string DataStreamBuffer { get; set; }

    public int GetStartMarker(int distinctCharactersRequired)
    {
        var buffer = new Queue<char>();
        for (int i = 0; i < DataStreamBuffer.Length; i++)
        {
            buffer.Enqueue(DataStreamBuffer[i]);
            if (buffer.Count > distinctCharactersRequired)
            {
                buffer.Dequeue();
            }

            if (buffer.Count == distinctCharactersRequired)
            {
                if (buffer.Distinct().Count() == distinctCharactersRequired) return ++i;
            }
        }

        return -1;
    }
}