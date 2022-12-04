using AdventOfCode2022.Day.Four;
using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Execution._2022
{
    public class AdventOfCodeExecutionDay4 : AdventOfCodeExecutionBase
    {
        public AdventOfCodeExecutionDay4(ITestOutputHelper output) : base(output, "4") { }

        [Theory]
        [InlineData()]
        public void Day4_Validation_1()
        {
        }

        [Theory]
        [InlineData(@"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8", 4)]
        public void Day4_Validation_2(string input, int expectedOverlaps)
        {
            input.Split(Environment.NewLine)
                .Select(x => new Pair(x))
                .Count(x => x.Overlaps())
                .Should().Be(expectedOverlaps);
        }

        [Fact]
        public async void Day_4_1()
        {            
            output.WriteLine($"{(await GetInput()).Select(p => new Pair(p)).Count(x => x.FullyContains())}");
        }

        [Fact]
        public async void Day_4_2()
        {
            output.WriteLine($"{(await GetInput()).Select(p => new Pair(p)).Count(x => x.Overlaps())}");
        }
    }
}