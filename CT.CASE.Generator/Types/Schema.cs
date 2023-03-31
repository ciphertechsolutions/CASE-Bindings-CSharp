using CT.CASE.Generator.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace CT.CASE.Generator.Types
{
    public sealed class Schema
    {
        private List<Class> _Classes;
        public IEnumerable<Class> Classes => _Classes;

        private List<OpenVocabulary> _OpenVocabularies;
        public IEnumerable<OpenVocabulary> OpenVocabularies => _OpenVocabularies;

        private Schema() { }

        public static Schema Load()
        {
            using (var graph = new Graph())
            {
                new TurtleParser().Load(graph, new StringReader(Resources.Case_1_2_0));
                return new Schema
                {
                    _Classes = GetClasses(graph),
                    _OpenVocabularies = GetVocabularies(graph),
                };
            }
        }
        
        private static List<OpenVocabulary> GetVocabularies(Graph graph)
        {
            var parser = new SparqlQueryParser();

            var enumQuery = parser.ParseFromString(Resources.EnumQuery);
            var caseEnums = (SparqlResultSet) graph.ExecuteQuery(enumQuery);
            var vocabs = caseEnums.Results.GroupBy(r => r["alias"]).Select(y => y.First());

            var parsedVocabs = new List<OpenVocabulary>();
            foreach (var vocab in vocabs)
            {
                var vocabMembers = caseEnums.Results
                    .Where(r => r["alias"] == vocab["alias"])
                    .Select(m => m["list_item"])
                    .Select(node => (ILiteralNode)node)
                    .Select(literal => literal.Value)
                    .Select(value => new OpenVocabulary.Member(value));

                parsedVocabs.Add(new OpenVocabulary(
                            // Alias else enum_uri if null ??
                            new Iri(vocab["alias"].ToString()),
                            vocabMembers));
            }
            return parsedVocabs;
        }

        private static List<Class> GetClasses(Graph graph)
        {
            SparqlQueryParser parser = new SparqlQueryParser();

            SparqlQuery classQuery = parser.ParseFromString(Resources.ClassQuery);
            SparqlResultSet classes = (SparqlResultSet)graph.ExecuteQuery(classQuery);
            var caseClasses = classes.Results.GroupBy(r => r["class"]).Select(y => y.First());

            SparqlQuery propertyQuery = parser.ParseFromString(Resources.PropertiesQuery);
            SparqlResultSet properties = (SparqlResultSet)graph.ExecuteQuery(propertyQuery);

            List<Class> parsedClasses = new List<Class>();
            foreach (var caseClass in caseClasses)
            {
                var classProps = properties.Results.FindAll(pr => pr["class"] == caseClass["class"]);

                List<Field> fields = new List<Field>();
                foreach (var prop in classProps)
                {
                    // TODO add this behavior to the documentation as a known limitation
                    // Only add properties that have types
                    if (prop.HasValue("target"))
                    {
                        string min = prop.HasValue("min") ? ((ILiteralNode)prop["min"]).Value : "0";
                        string max = prop.HasValue("max") ? ((ILiteralNode)prop["max"]).Value : "inf";

                        fields.Add(new Field(
                            new Iri(prop["path_uri"].ToString()),
                            new Iri(prop["target"].ToString()),
                            ParseCardinality(min, max)));
                    }
                }

                Iri parentClassIri;
                if (caseClass.TryGetBoundValue("subClassOf", out INode parentClassNode))
                {
                    parentClassIri = new Iri(parentClassNode.ToString());
                }
                else
                {
                    parentClassIri = null;
                }

                string comment = caseClass.HasValue("comment") ? ((ILiteralNode)caseClass["comment"]).Value : null;

                parsedClasses.Add(
                    new Class(new Iri(caseClass["class"].ToString()),
                    parentClassIri,
                    comment,
                    fields));
            }
            return parsedClasses;
        }

        private static Cardinality ParseCardinality(string min, string max)
        {
            if (max == "0")
                return Cardinality.Zero;
            else if (min == "1" && max == "1")
                return Cardinality.One;
            else if (min == "0" && max == "1")
                return Cardinality.ZeroOrOne;
            else
                return Cardinality.ZeroOrMore;
        }
    }
}
