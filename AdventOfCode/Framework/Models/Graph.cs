using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Framework.Models
{
    public class Graph<T>
    {
        private Coord<T>[,] graph;

        public Graph(T[,] data) : this(data.GetLength(0), data.GetLength(1))
        {
            var v = (
                from y in Enumerable.Range(0, data.GetLength(1))
                from x in Enumerable.Range(0, data.GetLength(0))
                select data[x, y]
            );
        }

        public Graph(int x, int y)
        {
            graph = new Coord<T>[x, y];
        }
    }
}
