using AdventOfCode2022.Day.Three;
using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Execution._2022
{
    public class AdventOfCodeExecutionDay3 : AdventOfCodeExecutionBase
    {
        public AdventOfCodeExecutionDay3(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(157, "vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg", "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
        public void Day3_Validation_1(int expectedSum, params string[] rucksacks)
        {
            rucksacks.ToList()
                .Select(Rucksack.GetDuplicates)
                .Select(Rucksack.GetPriority)
                .Sum()
                .Should().Be(expectedSum);
        }

        [Theory]
        [InlineData('r', 18, "vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg")]
        [InlineData('Z', 52, "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
        public void Day3_Validation_2(char Badge, int Priority, params string[] rucksacks)
        {
            var badge = Rucksack.GetGroupIdentifier(rucksacks);
            var sum = Rucksack.GetPriority(new[] { badge });

            badge.Should().Be(Badge);
            sum.Should().Be(Priority);
        }

        [Fact]
        public void Day_3_1()
        {
            using var sr = new StreamReader(@"..\..\..\Inputs\2022\day3input1.txt");
            string line;

            int sum = 0;
            while ((line = sr.ReadLine()) != null)
            {
                sum += Rucksack.CalculatePriority(line);
            }

            output.WriteLine($"{sum}");
        }

        [Fact]
        public void Day_3_2()
        {
            using var sr = new StreamReader(@"..\..\..\Inputs\2022\day3input1.txt");
            string line;

            var groups = new List<char>();
            var currentGroup = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                currentGroup ??= new List<string>();
                currentGroup.Add(line);

                if (currentGroup.Count == 3)
                {
                    groups.Add(Rucksack.GetGroupIdentifier(currentGroup));
                    currentGroup = null;
                }
            }

            output.WriteLine($"{groups.Select(x => Rucksack.GetPriority(new[] { x })).Sum()}");
        }
    }
}