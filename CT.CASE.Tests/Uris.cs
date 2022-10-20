using System;
using VDS.RDF;

namespace CT.CASE.Tests
{
    internal static class Uris
    {
        /// <summary>
        /// <c>rdf:type</c>, the predicate representing an "IS-A" relationship. This is
        /// also the IRI used by the <c>a</c> keyword in Turtle syntax.
        /// </summary>
        internal static readonly Uri RDF_TYPE = UriFactory.Create("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

        /// <summary>
        /// <c>xsd:string</c>, the IRI representing a string literal.
        /// </summary>
        internal static readonly Uri XSD_STRING = Xsd("string");

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>core:</c> prefix.
        /// </summary>
        internal static Uri Xsd(string suffix)
        {
            return UriFactory.Create("http://www.w3.org/2001/XMLSchema#" + suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>action:</c> prefix.
        /// </summary>
        internal static Uri Action(string suffix)
        {
            return Uco("action", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>core:</c> prefix.
        /// </summary>
        internal static Uri Core(string suffix)
        {
            return Uco("core", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>investigation:</c> prefix.
        /// </summary>
        internal static Uri Investigation(string suffix)
        {
            return Case("investigation", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>location:</c> prefix.
        /// </summary>
        internal static Uri Location(string suffix)
        {
            return Uco("location", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>marking:</c> prefix.
        /// </summary>
        internal static Uri Marking(string suffix)
        {
            return Uco("marking", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>observable:</c> prefix.
        /// </summary>
        internal static Uri Observable(string suffix)
        {
            return Uco("observable", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>vocab:</c> prefix.
        /// </summary>
        internal static Uri Vocab(string suffix)
        {
            return Case("vocabulary", suffix);
        }

        /// <summary>
        /// Append the given <c>suffix</c> to the <c>vocabulary:</c> prefix.
        /// </summary>
        internal static Uri Vocabulary(string suffix)
        {
            return Uco("vocabulary", suffix);
        }

        private static Uri Case(string path, string suffix)
        {
            return UriFactory.Create($"https://ontology.caseontology.org/case/{path}/{suffix}");
        }

        private static Uri Uco(string path, string suffix)
        {
            return UriFactory.Create($"https://ontology.unifiedcyberontology.org/uco/{path}/{suffix}");
        }
    }
}
