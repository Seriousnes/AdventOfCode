using AdventOfCode.Execution._2022;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace AdventOfCode.Execution.Framework;

public static class Extensions
{
    public static bool In<T>(this T value, params T[] values) => values.Contains(value);
    public static bool IsBetween<T>(this T value, T low, T high, bool lowerBoundInclusive = true, bool upperBoundInclusive = true) where T : IComparable
    {
        return
            lowerBoundInclusive switch
            {
                true => value.CompareTo(low) >= 0,
                false => value.CompareTo(low) > 0,
            }
            &&
            upperBoundInclusive switch
            {
                true => value.CompareTo(high) <= 0,
                false => value.CompareTo(high) < 0
            };
    }

    public static TItem[,] To2DArray<TItem, TSelector>(this IEnumerable<string> lines, Func<string, IEnumerable<TSelector>> splitRow, Func<TSelector, TItem> convertToT)
    {
        return lines.To2DArray(splitRow, (selector, _, _) => convertToT(selector));
    }

    public static TItem[,] To2DArray<TItem, TSelector>(this IEnumerable<string> lines, Func<string, IEnumerable<TSelector>> splitRow, Func<TSelector, int, int, TItem> convertToT)
    {
        var projectedLines = lines.Select(x => splitRow(x).ToList()).ToList();

        var result = new TItem[projectedLines.Count, projectedLines.First().Count];
        Parallel.ForEach(projectedLines, (line, _, x) =>
        {
            Parallel.ForEach(line, (item, _, y) =>
            {
                result[x, y] = convertToT(item, (int)x, (int)y);
            });
        });

        return result;
    }

    public static IEnumerable<string> ToLines(this string s) => s.Split(Environment.NewLine);
}