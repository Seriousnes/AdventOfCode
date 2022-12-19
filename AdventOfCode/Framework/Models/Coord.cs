using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Framework.Models;

public class Coord<T>
{
    public Coord(T value, int x, int y)
    { 
        this.Value = value;
        this.X = x;
        this.Y = y;
    }

    public T Value { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public static bool operator ==(Coord<T> lhs, Coord<T> rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Coord<T> lhs, Coord<T> rhs) => !(lhs.X == rhs.X && lhs.Y == rhs.Y);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return obj is Coord<T> coord && coord == this;
    }
}