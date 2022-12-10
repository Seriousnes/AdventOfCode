using System.Runtime.CompilerServices;

namespace AdventOfCode.Execution.Framework;

public static class Extensions
{
    public static bool In<T>(this T value, params T[] values) => values.Contains(value);
}
