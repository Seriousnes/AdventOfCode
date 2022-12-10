using System.Diagnostics;

namespace AdventOfCode.Execution._2022
{
    public class Day1 : AdventOfCodeExecutionBase
    {
        public Day1(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task Part1_Execution()
        {
            await Solve((lines) =>
            {
                var elves = new List<Elf>();
                Elf elf = null;
                foreach (var line in lines)
                {

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        elf = null;
                        continue;
                    }

                    if (elf is null)
                    {
                        elf = new Elf();
                        elves.Add(elf);
                    }

                    elf.Food.Add(int.Parse(line));
                }

                return elves.Max(x => x.Calories);
            });
        }

        [Fact]
        public async Task Part2_Execution()
        {
            await Solve((lines) =>
            {
                var elves = new List<Elf>();
                Elf elf = null;
                foreach (var line in lines)
                {

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        elf = null;
                        continue;
                    }

                    if (elf is null)
                    {
                        elf = new Elf();
                        elves.Add(elf);
                    }

                    elf.Food.Add(int.Parse(line));
                }

                return elves.OrderByDescending(x => x.Calories).Take(3).Sum(x => x.Calories);
            });
        }
    }
}

[DebuggerDisplay("{Calories} from {Food.Count} food items")]
public class Elf
{
    public List<int> Food { get; set; } = new();
    public int Calories => Food.Sum();
}