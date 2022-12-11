//using AdventOfCode2022.Day.X;

using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Execution._2015;

public class Day04 : AdventOfCodeExecutionBase
{
    public Day04(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part1_Validation(string input, int expectedValue)
    {
    }

    [Theory]
    [InlineData(@"", null)]
    public void Part2_Validation(string input, int expectedValue)
    {
    }

    [Fact]
    public async void Part1_Execution()
    {
        await Solve<object>((lines) =>
        {
            return GetLowestNumberForLeadingZeroCount(lines, 5);
        });
    }

    [Fact]
    public async void Part2_Execution()
    {
        await Solve<object>((lines) =>
        {
            return GetLowestNumberForLeadingZeroCount(lines, 6);
        });
    }

    private int GetLowestNumberForLeadingZeroCount(string[] input, int numberOfZeroes)
    {
        var md5 = MD5.Create();

        var matchString = new string('0', numberOfZeroes);
        
        var secretKey = input[0];
        var value = 1;
        do
        {

            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes($"{secretKey}{value}"));
            var hashValue = Convert.ToHexString(hash);
            if (hashValue.Substring(0, numberOfZeroes) ==  matchString)
                return value;
            else
                value++;
        } while (true);
    }
}