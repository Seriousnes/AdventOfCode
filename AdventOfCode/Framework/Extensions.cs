using AdventOfCode.Execution._2022;
using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Execution.Framework;

public static class Extensions
{
    public static bool In<T>(this T value, params T[] values) => values.Contains(value);

    public static TItem[,] To2DArray<TItem, TSelector>(this IEnumerable<string> lines, Func<string, IEnumerable<TSelector>> splitRow, Func<TSelector, TItem> convertToT)
    {
        return lines.To2DArray(splitRow, (selector, _, _) => convertToT(selector));
    }

    public static TItem[,] To2DArray<TItem, TSelector>(this IEnumerable<string> lines, Func<string, IEnumerable<TSelector>> splitRow, Func<TSelector, int, int, TItem> convertToT)
    {
        var width = lines.First().Length;
        var height = lines.Count();

        var result = new TItem[width, height];

        for (int y = 0; y < height; y++)
        {
            var row = splitRow(lines.ElementAt(y)).ToArray();
            for (int x = 0; x < row.Length; x++)
            {
                result[x, y] = convertToT(row[x], x, y);
            }
        }

        return result;
    }

    public static IEnumerable<string> ToLines(this string s) => s.Split(Environment.NewLine);
}
