using System;
using System.Collections.Generic;

namespace CT.CASE.Generator
{
    public sealed class Iri : IEquatable<Iri>
    {
        public static readonly string XSD_PREFIX = "http://www.w3.org/2001/XMLSchema#";

        private readonly string FullValue;

        public readonly string Prefix;
        public readonly string Suffix;

        private static readonly char[] PUNCT = new[] { '/', '#' };

        internal Iri(string fullIri)
        {
            FullValue = fullIri ?? throw new ArgumentNullException("fullIri");
            var index = FullValue.LastIndexOfAny(PUNCT);
            Prefix = FullValue.Substring(0, index + 1);
            Suffix = FullValue.Substring(index + 1);
        }

        public override string ToString()
        {
            return FullValue;
        }

        /// <summary>
        /// Checks whether this IRI refers to an RDF type
        /// that maps to a C# primitive.
        /// </summary>
        internal bool IsPrimitive()
        {
            return (Prefix == XSD_PREFIX) && !(Suffix == "string" || Suffix == "hexBinary" || Suffix == "anyURI");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Iri);
        }

        public bool Equals(Iri other)
        {
            return !(other is null) &&
                   FullValue == other.FullValue;
        }

        public override int GetHashCode()
        {
            return 2085469117 + EqualityComparer<string>.Default.GetHashCode(FullValue);
        }

        public static bool operator ==(Iri left, Iri right)
        {
            return EqualityComparer<Iri>.Default.Equals(left, right);
        }

        public static bool operator !=(Iri left, Iri right)
        {
            return !(left == right);
        }
    }
}
