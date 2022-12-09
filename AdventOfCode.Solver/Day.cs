namespace AdventOfCode.Solver
{
    public interface IDay
    {
        void Solve();
    }

    public abstract class Day<T> : IDay
    {
        public T Result { get; set; }

        public virtual void Solve()
        {

        }
    }
}