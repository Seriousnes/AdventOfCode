using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Execution._2015;

public class Day05 : AdventOfCodeExecutionBase
{
    public Day05(ITestOutputHelper output) : base(output)
    {
    }

    private static IEnumerable<NaughtyOrNiceRule> part1rules = new[]
    {
        new NaughtyOrNiceRule { Rule = (x) => x.Where(c => c.In('a', 'e', 'i', 'o', 'u')).Count() >= 3, Message = "Must contain 3 or more vowels" },
        new NaughtyOrNiceRule { Rule = (x) => x.Where((c, i) => (i + 1) < x.Length && c == x[i + 1]).Count() >= 1, Message = "Must contain at least one letter that appears twice in a row"},
        new NaughtyOrNiceRule { Rule = (x) => new[] { "ab", "cd", "pq", "xy" }.Select(s => !x.Contains(s)).All(x => x), Message = "Must not contain \"ab\", \"cd\", \"pq\", or \"xy\""}
    };

    private static IEnumerable<NaughtyOrNiceRule> part2rules = new[]
    {
        new NaughtyOrNiceRule { Rule = (x) =>
            {
                return x.Select((c, i) => (i + 1) < x.Length ? new string(new[] { c, x[i + 1]}) : null)
                    .Where(s => !string.IsNullOrEmpty(x))
                    .GroupBy(pair => pair)
                    .Where(group => group.Count() >= 2)
                    .Select(group => group.First())
                    .Select(s =>
                    {
                        var index = x.IndexOf(s);
                        return new { First = index, Last = x.IndexOf(s, index + 2) };
                    })
                    .Any(g => g.Last > 0);
            }, 
            Message = "Must contain a pair of any two letters that appears at least twice in the string without overlapping" },
        new NaughtyOrNiceRule { Rule = (x) => x.Where((c, i) => (i + 2) < x.Length && c == x[i + 2]).Count() >= 1, Message = "Must contain at least one letter which repeats with exactly one letter between them"},
    };

    [Theory]
    [InlineData(@"ugknbfddgicrmopn", true)]
    [InlineData(@"aaa", true)]
    [InlineData(@"jchzalrnumimnmhp", false)]
    [InlineData(@"haegwjzuvuyypxyu", false)]
    [InlineData(@"dvszwmarrgswjxmb", false)]
    public void Part1_Validation(string input, bool expectedValue)
    {
        var validator = new NaughtyOrNiceValidator(part1rules);
        var validationResult = validator.Validate(input);
        validationResult.IsValid.Should().Be(expectedValue);
        if (validationResult.Errors.Any())
        {
            output.WriteLine($"{input} is naughty because:");
            foreach (var error in validationResult.Errors)
                output.WriteLine($"\t{error.ErrorMessage}");
        }
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            var validator = new NaughtyOrNiceValidator(part1rules);
            int niceStrings = 0;
            foreach (var line in lines)
            {
                if (validator.Validate(line).IsValid) niceStrings++;
            }
            return niceStrings;
        });
    }

    [Theory]
    [InlineData(@"qjhvhtzxzqqjkmpb", true)]
    [InlineData(@"xxyxx", true)]
    [InlineData(@"uurcxstgmygtbstg", false)]
    [InlineData(@"ieodomkazucvgmuy", false)]
    [InlineData(@"aaa", false)]
    [InlineData(@"xyxyaoa", true)]
    [InlineData(@"abcdefeghicd", true)]
    public void Part2_Validation(string input, bool expectedValue)
    {
        var validator = new NaughtyOrNiceValidator(part2rules);
        var validationResult = validator.Validate(input);
        if (validationResult.Errors.Any())
        {
            output.WriteLine($"{input} is naughty because:");
            foreach (var error in validationResult.Errors)
                output.WriteLine($"\t{error.ErrorMessage}");
        }
        validationResult.IsValid.Should().Be(expectedValue);
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            var validator = new NaughtyOrNiceValidator(part2rules);
            int niceStrings = 0;
            foreach (var line in lines)
            {
                if (validator.Validate(line).IsValid)
                    niceStrings++;
            }
            return niceStrings;
        });
    }
}


public class NaughtyOrNiceValidator : AbstractValidator<string>
{
    public NaughtyOrNiceValidator(IEnumerable<NaughtyOrNiceRule> rules)
    {
        // apply rules
        foreach (var rule in rules)
        {
            RuleFor(x => x)
                .Must(rule.Rule)
                .WithMessage(rule.Message);
        }
    }
}

public class NaughtyOrNiceRule
{
    public Func<string, bool> Rule;
    public string Message { get; set; }
}