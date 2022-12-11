using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode.Execution._2022;

public class Day11 : AdventOfCodeExecutionBase
{
    public Day11(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1", 10605)]
    public void Part1_Validation(string input, long expectedValue)
    {
        var lines = input.Split(Environment.NewLine);
        var m = new MonkeyFactory() { WorryManager = (_, x) => (long)(x / 3.0) };
        m.ParseMonkeys(lines);

        m.HandleItems(20);

        m.GetMonkeyBusiness().Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(@"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1", 2713310158)]
    public void Part2_Validation(string input, long expectedValue)
    {
        var lines = input.Split(Environment.NewLine);

        var mf = new MonkeyFactory();
        mf.ParseMonkeys(lines);

        mf.WorryManager = (m, worryLevel) => worryLevel % mf.Monkies.Aggregate(1L, (p, m) => p * m.Test);

        mf.HandleItems(10000);
        mf.GetMonkeyBusiness().Should().Be(expectedValue);
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var mf = new MonkeyFactory() { WorryManager = (_, x) => (long)(x / 3.0) };
            mf.ParseMonkeys(lines);
            mf.HandleItems(20);
            return mf.GetMonkeyBusiness();
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var mf = new MonkeyFactory();
            mf.ParseMonkeys(lines);

            var worryFactor = mf.Monkies.Aggregate(1L, (p, m) => p * m.Test);

            mf.WorryManager = (m, worryLevel) => worryLevel % worryFactor;
            mf.HandleItems(10000);
            return mf.GetMonkeyBusiness();
        });
    }
}

public class MonkeyFactory
{
    public List<Monkey> Monkies { get; set; } = new();
    public void ParseMonkeys(IEnumerable<string> input)
    {
        Monkey m = null;
        foreach (var line in input)
        {
            if (m is null)
            {
                m = new Monkey();
                Monkies.Add(m);
            }

            if (string.IsNullOrWhiteSpace(line))
                m = null;
            else
                UpdateMonkey(m, line);
        }
    }

    private void UpdateMonkey(Monkey monkey, string value)
    {
        var m = Regex.Match(value, @"Monkey\s(?'id'\d+)");
        if (m.Success)
        {
            monkey.Id = int.Parse(m.Groups["id"].Value);
            return;
        }

        m = Regex.Match(value, @"Starting items: (?'items'.*)");
        if (m.Success)
        {
            monkey.Items.AddRange(m.Groups["items"].Value.Split(", ").Select(long.Parse));
            return;
        }

        m = Regex.Match(value, @"Operation: new = (?'operation'.*)");
        if (m.Success)
        {
            monkey.Operation = m.Groups["operation"].Value.GetOperation();
            return;
        }

        m = Regex.Match(value, @"Test: divisible by (?'test'\d+)");
        if (m.Success)
        {
            monkey.Test = int.Parse(m.Groups["test"].Value);
            return;
        }

        m = Regex.Match(value, @"If true: throw to monkey (?'monkey'.*)");
        if (m.Success)
        {
            monkey.trueMonkey = int.Parse(m.Groups["monkey"].Value);
            return;
        }

        m = Regex.Match(value, @"If false: throw to monkey (?'monkey'.*)");
        if (m.Success)
        {
            monkey.falseMonkey = int.Parse(m.Groups["monkey"].Value);
            return;
        }
    }

    public void HandleItems(int numRounds)
    {
        for (int round = 1; round <= numRounds; round++)
        {
            foreach (var m in Monkies)
            {
                InspectItems(m);
            }
        }
    }

    private void InspectItems(Monkey monkey)
    {
        while (monkey.Items.Count > 0)
        {
            var item = monkey.Items[0];
            var worryLevel = WorryManager(monkey, monkey.Operation(item));

            monkey.ThrowToMonkey(
                worryLevel % (long)monkey.Test == 0 ?
                Monkies.Single(x => x.Id == monkey.TestResult.IfTrue) :
                Monkies.Single(x => x.Id == monkey.TestResult.IfFalse),
                worryLevel);
        }
    }

    public Func<Monkey, long, long> WorryManager { get; set; }

    public long GetMonkeyBusiness()
    {
        var mostActiveMonkies = Monkies.OrderByDescending(x => x.ItemsCounted).Take(2).ToArray();
        return (long)mostActiveMonkies[0].ItemsCounted * (long)mostActiveMonkies[1].ItemsCounted;
    }
}

public class Monkey
{
    public int trueMonkey { get; set; }
    public int falseMonkey { get; set; }

    public int Id { get; set; }
    public List<long> Items { get; } = new();
    public Func<long, long> Operation { get; set; }
    public int Test { get; set; }
    public (int IfTrue, int IfFalse) TestResult => (trueMonkey, falseMonkey);
    public int ItemsCounted { get; set; } = 0;

    public void ThrowToMonkey(Monkey m, long item)
    {
        m.Catch(item);
        Items.RemoveAt(0);
        ItemsCounted++;
    }

    public void Catch(long item)
    {
        Items.Add(item);
    }
}

public static class Day11Extensions
{
    public static Func<long, long> GetOperation(this string value)
    {
        var operationParts = value.Split(' ');

        var (left, right) = (GetExpression(operationParts[0]), GetExpression(operationParts[^1]));
        if (right is ParameterExpression p && p.Name == ((ParameterExpression)left).Name)
            right = left;

        Expression expression = operationParts[1] switch
        {
            "*" => Expression.Multiply(left, right),
            "+" => Expression.Add(left, right),
            "-" => Expression.Subtract(left, right),
            "/" => Expression.Divide(left, right),
            _ => throw new ArgumentException($"{operationParts[1]} is not a valid operation")
        };

        return Expression.Lambda<Func<long, long>>(expression, new[] { (ParameterExpression)left }).Compile();
    }

    private static Expression GetExpression(string value)
    {
        return value switch
        {
            var v when value.In("new", "old") => Expression.Parameter(typeof(long), v),
            var _ when long.TryParse(value, out var @int) => Expression.Constant(@int, typeof(long)),
            _ => throw new ArgumentException($"{value} is not a valid expression operation", nameof(value))
        };
    }
}