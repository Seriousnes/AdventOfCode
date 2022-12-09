namespace AdventOfCode2022.Day.Two;
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

public static class Day2
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
