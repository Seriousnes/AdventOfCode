namespace AdventOfCode.Execution.Framework;

public class AdventOfCodeAttribute : Attribute
{
    public AdventOfCodeAttribute(int year, int day)
    {
        this.Year = year;
        Day = day;
    }

    public int Year { get; }
    public int Day { get; }
}