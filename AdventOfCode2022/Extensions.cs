using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class Extensions
    {
        public static bool In<T>(this T value, params T[] values) => values.Contains(value);
    }
}
