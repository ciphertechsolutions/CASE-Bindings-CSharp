using System;

namespace CT.CASE.Generator.Types
{
    public enum Cardinality
    {
        Zero,
        One,
        ZeroOrOne,
        ZeroOrMore,
    }

    public static class CardinalityMethods
    {
        /// <summary>
        /// Calculates the set-theoretic intersection of two cardinalities. This is the most inclusive cardinality
        /// that is a subset of each of the parameters.
        /// </summary>
        public static Cardinality Intersection(this Cardinality a, Cardinality b)
        {
            // Arrange things so that a < b.
            if (a == b)
                return a;
            if (a > b)
                return b.Intersection(a);

            // In most cases, each Cardinality is a superset of all Cardinality values declared before it,
            // so the intersection operation is "just" the minimum operation. The only exception is One,
            // which is not a superset of zero.
            if (a == Cardinality.Zero && b == Cardinality.One)
            {
                throw new EmptyIntersectionException();
            }
            return a;
        }
    }

    public sealed class EmptyIntersectionException : ApplicationException
    {
        public EmptyIntersectionException() : base() { }
    }
}
