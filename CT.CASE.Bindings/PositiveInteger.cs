using System.Numerics;
using VDS.RDF;

namespace CT.CASE.Bindings
{
    public struct PositiveInteger
    {
        private BigInteger Value;

        public PositiveInteger(BigInteger value)
        {
            if (value <= BigInteger.Zero)
                throw new System.ArgumentOutOfRangeException("value", value, "PositiveInteger values must be greater than BigInteger.Zero");
            Value = value;
        }

        internal INode ToNode(Graph graph)
        {
            return graph.CreateLiteralNode(Value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#positiveInteger"));
        }
    }
}
