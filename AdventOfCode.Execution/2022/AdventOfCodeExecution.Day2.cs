using AdventOfCode2022.Day.Two;
using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Execution._2022
{
    public class AdventOfCodeExecutionDay2 : AdventOfCodeExecutionBase
    {
        public AdventOfCodeExecutionDay2(ITestOutputHelper output) : base(output, "2") { }

        [Theory]
        [InlineData(Play.Rock, Play.Paper, 8)]
        [InlineData(Play.Paper, Play.Rock, 1)]
        [InlineData(Play.Scissors, Play.Scissors, 6)]
        public void Day2_Validation_1(Play A, Play B, int expectedScore)
        {
            var round = new Round { Opponent = A, Picking = B };
            round.Points().Should().Be(expectedScore);
        }

        [Theory]
        [InlineData(Play.Rock, Result.Draw, 4)]
        [InlineData(Play.Paper, Result.Loss, 1)]
        [InlineData(Play.Scissors, Result.Win, 7)]
        public void Day2_Validation_2(Play A, Result B, int expectedScore)
        {
            var round = new Round_v2 { Opponent = A, ExpectedResult = B };
            round.Points().Should().Be(expectedScore);
        }

        [Fact]
        public void Day_2_1()
        {
            using var sr = new StreamReader(@"..\..\..\Inputs\2022\day2input1.txt");
            string line;

            int score = 0;
            while ((line = sr.ReadLine()) != null)
            {
                var lineParts = line.Split(' ');
                var round = new Round { Opponent = Day2.PlayFromString(lineParts[0]), Picking = Day2.PlayFromString(lineParts[1]) };
                score += round.Points();
            }

            output.WriteLine($"{score}");
        }

        [Fact]
        public void Day_2_2()
        {
            using var sr = new StreamReader(@"..\..\..\Inputs\2022\day2input1.txt");
            string line;

            int score = 0;
            while ((line = sr.ReadLine()) != null)
            {
                var lineParts = line.Split(' ');
                var round = new Round_v2 { Opponent = Day2.PlayFromString(lineParts[0]), ExpectedResult = Day2.ResultFromString(lineParts[1]) };
                score += round.Points();
            }

            output.WriteLine($"{score}");
        }
    }
}