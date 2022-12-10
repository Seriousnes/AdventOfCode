namespace AdventOfCode.Execution._2022;

public class Day2 : AdventOfCodeExecutionBase
{
    public Day2(ITestOutputHelper output) : base(output) { }

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
            var round = new Round { Opponent = RockPaperScissors.PlayFromString(lineParts[0]), Picking = RockPaperScissors.PlayFromString(lineParts[1]) };
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
            var round = new Round_v2 { Opponent = RockPaperScissors.PlayFromString(lineParts[0]), ExpectedResult = RockPaperScissors.ResultFromString(lineParts[1]) };
            score += round.Points();
        }

        output.WriteLine($"{score}");
    }
}

public class Round
{
    public Play Opponent { get; set; }
    public Play Picking { get; set; }

    public int Points()
    {
        return (int)Picking + (int)Picking.Beats(Opponent);
    }
}

public class Round_v2
{
    public Play Opponent { get; set; }
    public Result ExpectedResult { get; set; }

    public int Points()
    {
        return (int)ExpectedResult + (int)Opponent.ToGetResult(ExpectedResult);
    }
}

public enum Play
{
    [Result(Scissors, Paper)]
    Rock = 1,
    [Result(Rock, Scissors)]
    Paper = 2,
    [Result(Paper, Rock)]
    Scissors = 3,
}

public enum Result
{
    Loss = 0,
    Draw = 3,
    Win = 6
}


public class ResultAttribute : Attribute
{
    public ResultAttribute(Play beats, Play beatenBy)
    {
        Beats = beats;
        BeatenBy = beatenBy;
    }

    public Play Beats { get; set; }
    public Play BeatenBy { get; set; }
}

public static class RockPaperScissors
{
    public static Play PlayFromString(string s) => s switch
    {
        _ when s.In("A", "X") => Play.Rock,
        _ when s.In("B", "Y") => Play.Paper,
        _ when s.In("C", "Z") => Play.Scissors,
    };

    public static Result ResultFromString(string s) => s switch
    {
        "X" => Result.Loss,
        "Y" => Result.Draw,
        "Z" => Result.Win
    };

    public static Result Beats(this Play play, Play vs)
    {
        return play == vs ? Result.Draw : play.GetBeats() == vs ? Result.Win : Result.Loss;
    }

    public static Play ToGetResult(this Play play, Result expectedResult)
    {
        return expectedResult switch
        {
            Result.Loss => play.GetBeats(),
            Result.Draw => play,
            Result.Win => play.GetBeatenBy()
        };
    }

    public static Play GetBeats(this Play play)
    {
        var type = play.GetType();
        var memberInfo = type.GetMember(play.ToString());
        var beats = memberInfo[0].GetCustomAttribute<ResultAttribute>();
        return beats.Beats;
    }

    public static Play GetBeatenBy(this Play play)
    {
        var type = play.GetType();
        var memberInfo = type.GetMember(play.ToString());
        var beats = memberInfo[0].GetCustomAttribute<ResultAttribute>();
        return beats.BeatenBy;
    }
}
