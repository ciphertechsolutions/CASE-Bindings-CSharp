using CT.CASE.Bindings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Linq;
using VDS.RDF;

namespace CT.CASE.Tests
{
    [TestClass]
    public class DataSetTests
    {
        [TestMethod]
        public void EmptyDataSet()
        {
            var ds = new DataSet();
            var graph = ds.Graph;
        }

        [TestMethod]
        public void ClassTypeCreation()
        {
            var ds = new DataSet();

            Uri subjectUri = UriFactory.Create("http://test.example/data/cdf");
            var namedCdf = ds.CreateContentDataFacet(subjectUri, null);
            Assert.AreEqual(subjectUri, namedCdf.IdentifierUri());

            var anonCdf = ds.CreateContentDataFacet(null, null);
            Assert.AreEqual(null, anonCdf.IdentifierUri());
        }

        [TestMethod]
        public void RequiredPrimitiveField()
        {
            var dataSet = new DataSet();
            var license = dataSet.CreateLicenseMarking(UriFactory.Create("http://test.example/data/license"), "Public domain");
            Assert.IsTrue(dataSet.Graph.ContainsTriple(new Triple(
                dataSet.Graph.CreateUriNode(license.IdentifierUri()),
                dataSet.Graph.CreateUriNode(Uris.Marking("license")),
                dataSet.Graph.CreateLiteralNode("Public domain", Uris.XSD_STRING)
            )));
        }

        [TestMethod]
        public void RequiredPrimitiveFieldThrowsIfLeftOut()
        {
            var dataSet = new DataSet();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dataSet.CreateLicenseMarking(UriFactory.Create("http://test.example/data/license"), null);
            });
        }

        [TestMethod]
        public void OptionalPrimitiveFieldMayBeAbsent()
        {
            var dataSet = new DataSet();
            var provenance = dataSet.CreateProvenanceRecord(UriFactory.Create("http://test.example/data/provenance"), exhibitNumber: null, rootExhibitNumber: new string[0]);
            Assert.AreEqual(0, dataSet.Graph.GetTriplesWithSubjectPredicate(
                dataSet.Graph.CreateUriNode(provenance.IdentifierUri()),
                dataSet.Graph.CreateUriNode(Uris.Investigation("exhibitNumber"))).Count());
        }

        [TestMethod]
        public void OptionalPrimitiveFieldMayBePresent()
        {
            var dataSet = new DataSet();
            var provenance = dataSet.CreateProvenanceRecord(UriFactory.Create("http://test.example/data/provenance"), exhibitNumber: "test-exhibit-number", rootExhibitNumber: new string[0]);
            Assert.IsTrue(dataSet.Graph.ContainsTriple(new Triple(
                dataSet.Graph.CreateUriNode(provenance.IdentifierUri()),
                dataSet.Graph.CreateUriNode(Uris.Investigation("exhibitNumber")),
                dataSet.Graph.CreateLiteralNode("test-exhibit-number", Uris.XSD_STRING))));
        }

        [TestMethod]
        public void MultiPrimitiveFieldMayBeAbsent()
        {
            var dataSet = new DataSet();
            var provenance = dataSet.CreateProvenanceRecord(UriFactory.Create("http://test.example/data/provenance"), exhibitNumber: null, rootExhibitNumber: new string[0]);
            Assert.AreEqual(0, dataSet.Graph.GetTriplesWithSubjectPredicate(
                dataSet.Graph.CreateUriNode(provenance.IdentifierUri()),
                dataSet.Graph.CreateUriNode(Uris.Investigation("rootExhibitNumber"))).Count());
        }


        [TestMethod]
        public void MultiPrimitiveFieldMayHaveMultipleValues()
        {
            var dataSet = new DataSet();
            var provenance = dataSet.CreateProvenanceRecord(UriFactory.Create("http://test.example/data/provenance"), exhibitNumber: null, rootExhibitNumber: new string[]
            {
                "one-test-value", "another-test-value"
            });
            var subj = dataSet.Graph.CreateUriNode(provenance.IdentifierUri());
            var pred = dataSet.Graph.CreateUriNode(Uris.Investigation("rootExhibitNumber"));
            Assert.AreEqual(2, dataSet.Graph.GetTriplesWithSubjectPredicate(subj, pred).Count());
            Assert.IsTrue(dataSet.Graph.ContainsTriple(new Triple(subj, pred, dataSet.Graph.CreateLiteralNode("one-test-value", Uris.XSD_STRING))));
            Assert.IsTrue(dataSet.Graph.ContainsTriple(new Triple(subj, pred, dataSet.Graph.CreateLiteralNode("another-test-value", Uris.XSD_STRING))));
        }

        [TestMethod]
        public void ObjectValuedField()
        {
            var ds = new DataSet();
            var investigationUri = UriFactory.Create("http://test.example/data/investigation");
            var authorizationUri = UriFactory.Create("http://test.example/data/authorization");

            var authorization = ds.CreateAuthorization(authorizationUri);
            var investigation = ds.CreateInvestigation(investigationUri, relevantAuthorization: new[] { authorization });

            var s = ds.Graph.CreateUriNode(investigationUri);
            var p = ds.Graph.CreateUriNode(Uris.Investigation("relevantAuthorization"));
            var o = ds.Graph.CreateUriNode(authorizationUri);
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void SubclassesInheritProperties()
        {
            var ds = new DataSet();

            // Do the factory methods include parameters from the ancestors?
            var assertion = ds.CreateAssertion(
                rdfIdentifier: UriFactory.Create("http://test.example/data/assertion"),
                // UcoObject arguments
                tag: new[] { "test-tag" },
                // Assertion arguments
                statement: new[] { "test statement" }
            );

            // Is the returned object assignable to the ancestor types?
            Assert.IsInstanceOfType(assertion, typeof(Assertion));
            Assert.IsInstanceOfType(assertion, typeof(UcoObject));

            // Do inherited properties make it into the RDF graph?
            var s = ds.Graph.CreateUriNode(assertion.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Core("tag"));
            var o = ds.Graph.CreateLiteralNode("test-tag", Uris.XSD_STRING);
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void OptionalVocabField()
        {
            OptionalVocabFieldImpl(EndiannessTypeVocab.BigEndian, "Big-endian");
            OptionalVocabFieldImpl(EndiannessTypeVocab.MiddleEndian, "Middle-endian");
            OptionalVocabFieldImpl(EndiannessTypeVocab.LittleEndian, "Little-endian");
            OptionalVocabFieldImpl(null, null);
        }

        public void OptionalVocabFieldImpl(EndiannessTypeVocab endianness, string lexical)
        {
            var dataSet = new DataSet();
            var cdf = dataSet.CreateContentDataFacet(UriFactory.Create("http://test.example/data/cdf"), byteOrder: endianness);

            var s = dataSet.Graph.CreateUriNode(cdf.IdentifierUri());
            var p = dataSet.Graph.CreateUriNode(Uris.Observable("byteOrder"));
            var actualObjects = dataSet.Graph.GetTriplesWithSubjectPredicate(s, p).Select((triple) => triple.Object);
            var expectedObjects = (lexical == null) ? Enumerable.Empty<INode>() : dataSet.Graph.CreateLiteralNode(lexical, Uris.Vocabulary("EndiannessTypeVocab")).AsEnumerable();
            Assert.IsTrue(
                actualObjects.SequenceEqual(expectedObjects),
                "expected\n    [{0}]\nbut got\n    [{1}]",
                string.Join(", ", expectedObjects.Select((obj) => obj.ToString())),
                string.Join(", ", actualObjects.Select((obj) => obj.ToString()))
            );
        }

        [TestMethod]
        public void OpenVocabsAreExtensible()
        {
            var dataSet = new DataSet();
            var cdf = dataSet.CreateContentDataFacet(
                UriFactory.Create("http://test.example/data/cdf"),
                byteOrder: new EndiannessTypeVocab("Custom-endian")
            );

            var s = dataSet.Graph.CreateUriNode(cdf.IdentifierUri());
            var p = dataSet.Graph.CreateUriNode(Uris.Observable("byteOrder"));
            var o = dataSet.Graph.CreateLiteralNode("Custom-endian", Uris.XSD_STRING);
            Assert.IsTrue(dataSet.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdStringMapsToString()
        {
            var ds = new DataSet();
            var authorization = ds.CreateAuthorization(UriFactory.Create("http://test.example/x"), authorizationType: "test-string");
            var s = ds.Graph.CreateUriNode(authorization.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Investigation("authorizationType"));
            var o = ds.Graph.CreateLiteralNode("test-string", Uris.XSD_STRING);
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdUnsignedIntMapsToUInt()
        {
            var ds = new DataSet();
            var thing = ds.CreateWindowsPEOptionalHeader(UriFactory.Create("http://test.example/x"), addressOfEntryPoint: 3u.AsEnumerable());
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("addressOfEntryPoint"));
            var o = ds.Graph.CreateLiteralNode("3", Uris.Xsd("unsignedInt"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdUnsignedShortMapsToUShort()
        {
            var ds = new DataSet();
            var thing = ds.CreateWindowsPEBinaryFileFacet(UriFactory.Create("http://test.example/x"), characteristics: ((ushort)3).AsEnumerable());
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("characteristics"));
            var o = ds.Graph.CreateLiteralNode("3", Uris.Xsd("unsignedShort"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdDateTimeMapsToDateTime()
        {
            var ds = new DataSet();
            var thing = ds.CreateAuthorization(UriFactory.Create("http://test.example/x"), startTime: new DateTime(1776, 7, 4, 12, 30, 0));
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Core("startTime"));
            var o = ds.Graph.CreateLiteralNode("1776-07-04T12:30:00.000000", Uris.Xsd("dateTime"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdNonNegativeIntegerMapsToCustomType()
        {
            var ds = new DataSet();
            var thing = ds.CreateAction(UriFactory.Create("http://test.example/x"), actionCount: new NonNegativeInteger(3));
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Action("actionCount"));
            var o = ds.Graph.CreateLiteralNode("3", Uris.Xsd("nonNegativeInteger"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdBooleanMapsToBool()
        {
            var ds = new DataSet();
            var thing = ds.CreateAccountFacet(UriFactory.Create("http://test.example/x"), isActive: true);
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("isActive"));
            var o = ds.Graph.CreateLiteralNode("true", Uris.Xsd("boolean"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdIntegerMapsToInt()
        {
            var ds = new DataSet();
            var thing = ds.CreateAlternateDataStreamFacet(UriFactory.Create("http://test.example/x"), size: 3);
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("size"));
            var o = ds.Graph.CreateLiteralNode("3", Uris.Xsd("integer"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdHexBinaryMapsToByteArray()
        {
            var ds = new DataSet();
            var thing = ds.CreateAndroidDeviceFacet(UriFactory.Create("http://test.example/x"), androidID: new byte[] {
                // Make sure to include a byte with a leading zero
                0xc0, 0x01, 0xc0, 0xde
            });
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("androidID"));
            var o = ds.Graph.CreateLiteralNode("c001c0de", Uris.Xsd("hexBinary"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdDecimalMapsToDecimal()
        {
            var ds = new DataSet();
            var thing = ds.CreateGPSCoordinatesFacet(UriFactory.Create("http://test.example/x"), hdop: 1.3m);
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Location("hdop"));
            var o = ds.Graph.CreateLiteralNode("1.3", Uris.Xsd("decimal"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdAnyUriMapsToUri()
        {
            var ds = new DataSet();
            var thing = ds.CreateExternalReference(UriFactory.Create("http://test.example/x"), referenceURL: UriFactory.Create("http://test.example/referent"));
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Core("referenceURL"));
            var o = ds.Graph.CreateLiteralNode("http://test.example/referent", Uris.Xsd("anyURI"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdByteMapsToByte()
        {
            var ds = new DataSet();
            var thing = ds.CreateWindowsPEOptionalHeader(UriFactory.Create("http://test.example/x"), majorLinkerVersion: ((sbyte)127).AsEnumerable());
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("majorLinkerVersion"));
            var o = ds.Graph.CreateLiteralNode("127", Uris.Xsd("byte"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdDurationMapsToTimeSpan()
        {
            var ds = new DataSet();
            var thing = ds.CreateURLVisitFacet(UriFactory.Create("http://test.example/x"), visitDuration: TimeSpan.FromSeconds(30));
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("visitDuration"));
            var o = ds.Graph.CreateLiteralNode("PT30S", Uris.Xsd("duration"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }

        [TestMethod]
        public void XsdPositiveIntegerMapsToCustomType()
        {
            var ds = new DataSet();
            var thing = ds.CreateSQLiteBlobFacet(UriFactory.Create("http://test.example/x"), rowIndex: new PositiveInteger(3).AsEnumerable());
            var s = ds.Graph.CreateUriNode(thing.IdentifierUri());
            var p = ds.Graph.CreateUriNode(Uris.Observable("rowIndex"));
            var o = ds.Graph.CreateLiteralNode("3", Uris.Xsd("positiveInteger"));
            Assert.IsTrue(ds.Graph.ContainsTriple(new Triple(s, p, o)));
        }
    }
}
