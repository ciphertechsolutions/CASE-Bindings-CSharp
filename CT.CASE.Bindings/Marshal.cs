using System;
using VDS.RDF;
using VDS.RDF.Nodes;

namespace CT.CASE.Bindings
{
    /// <summary>
    /// Methods for converting C# values into RDF nodes.
    /// </summary>
    internal static class Marshall
    {
        internal static void AddTriple(
            Graph graph,
            INode subject,
            INode predicate,
            INode obj)
        {
            if (obj == null)
                return;
            graph.Assert(subject, predicate, obj);
        }

        internal static INode ToNode(this bool value, Graph graph)
        {
            return new BooleanNode(graph, value);
        }

        internal static INode ToNode(this bool? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this byte[] value, Graph graph)
        {
            if (value == null)
                return null;
            var lexical = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            return graph.CreateLiteralNode(lexical, UriFactory.Create("http://www.w3.org/2001/XMLSchema#hexBinary"));
        }

        internal static INode ToNode(this string value, Graph graph)
        {
            if (value == null)
                return null;
            return graph.CreateLiteralNode(value, UriFactory.Create("http://www.w3.org/2001/XMLSchema#string"));
        }

        internal static INode ToNode(this decimal value, Graph graph)
        {
            return new DecimalNode(graph, value);
        }

        internal static INode ToNode(this decimal? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this sbyte value, Graph graph)
        {
            return graph.CreateLiteralNode(value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#byte"));
        }

        internal static INode ToNode(this sbyte? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this int value, Graph graph)
        {
            return graph.CreateLiteralNode(value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#integer"));
        }

        internal static INode ToNode(this int? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this uint value, Graph graph)
        {
            return graph.CreateLiteralNode(value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#unsignedInt"));
        }

        internal static INode ToNode(this uint? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this ushort value, Graph graph)
        {
            return graph.CreateLiteralNode(value.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#unsignedShort"));
        }

        internal static INode ToNode(this ushort? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this DateTime value, Graph graph)
        {
            return new DateTimeNode(graph, value);
        }

        internal static INode ToNode(this DateTime? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this TimeSpan value, Graph graph)
        {
            return new TimeSpanNode(graph, value);
        }

        internal static INode ToNode(this TimeSpan? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this PositiveInteger? value, Graph graph)
        {
            return value?.ToNode(graph);
        }

        internal static INode ToNode(this Uri uri, Graph graph)
        {
            return graph.CreateLiteralNode(uri.ToString(), UriFactory.Create("http://www.w3.org/2001/XMLSchema#anyURI"));
        }
    }
}
