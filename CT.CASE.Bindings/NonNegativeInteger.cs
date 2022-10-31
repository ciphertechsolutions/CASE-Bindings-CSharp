using System.Numerics;
using VDS.RDF;

namespace CT.CASE.Bindings
{
    public struct NonNegativeInteger
    {
        private BigInteger Value;

        public NonNegativeInteger(BigInteger value)
        {
            if (value < BigInteger.Zero)
                throw new System.ArgumentOutOfRangeException("value", value, "NonNegativeInteger values must be less than BigInteger.Zero");
            Value = value;
        }

        internal INode ToNode(Graph graph)
        {
            return graph.CreateLiteralNode(Value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#nonNegativeInteger"));
        }
    }
}
