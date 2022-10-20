# CASE Bindings for C#

Generated bindings for version 1.0.0 of the [CASE ontology](https://caseontology.org/index.html).

These bindings are write-only: they support creating a CASE-compliant RDF graph from scratch, but not editing or
inspecting existing graphs.

## Using the bindings

The entry point to the bindings is the `CT.CASE.Bindings.DataSet` class.

```cs
var ds = new DataSet();
```

The `DataSet` class exposes a factory method for each `owl:Class` in the ontology schema. The first parameter on each
method is the IRI of the resulting RDF subject; pass `null` to generate a blank node instead. The remaining parameters
map to the properties on the `Thing` being created.

**Optional** properties (those with `sh:maxCount "1"^^xsd:integer`) have a nullable type and default to `null`, which is
taken to mean "no value." **Repeated** properties (those with no `sh:maxCount` constraint) have type `IEnumerable<T>`
and also default to `null`, which is taken as equivalent to the empty `Enumerable`. **Required** properties
(`sh:minCount "1"^^xsd:integer`) have no default and must be supplied; a `null` argument here will throw a
`NullArgumentException`. Given the sheer number of parameters available on most factory methods, it is highly
recommended to use C#'s named arguments syntax for the property arguments:

```cs
// investigation:Authorization defines four properties:
//     core:endTime (type xsd:dateTime, max count 1)
//     core:startTime (type xsd:dateTime, max count 1)
//     investigation:authorizationType (type xsd:string, max count 1)
//     investigation:authorizationIdentifier (type xsd:string, no max count)
// and inherits ten more from core:UcoObject, including
//     core:description (type xsd:string, no max count)
Authorization auth = ds.CreateAuthorization(
    /* RDF identifier */ UriFactory.Create("http://data.example/00000000-0000-0000-0000-000000000000"),
    authorizationType: "documentation",
    authorizationIdentifier: new string[] { "00000000-0000-0000-0000-000000000000" },
    description: new string[] { "Created as an example." }
);
```

The returned object acts as a strongly-typed wrapper around the RDF node and can be passed to subsequent factory methods
to link nodes together.

```cs
// action:Action defines many properties, including
//     action:result (type core:UcoObject, which investigation:Authorization is a subClassOf)
Action authAction = ds.CreateAction(null, result: new UcoObject[] { auth });
```

RDF primitive types -- those in the `xsd:` namespace -- map to C# types according to the following table.

| RDF type                 | C# type               |
|--------------------------|-----------------------|
| `xsd:anyURI`             | `System.Uri`          |
| `xsd:boolean`            | `bool`                |
| `xsd:byte`               | `sbyte`               |
| `xsd:dateTime`           | `System.DateTime`     |
| `xsd:decimal`            | `decimal`             |
| `xsd:duration`           | `System.TimeSpan`     |
| `xsd:hexBinary`          | `byte[]`              |
| `xsd:integer`            | `int`                 |
| `xsd:nonNegativeInteger` | `NonNegativeInteger`¹ |
| `xsd:positiveInteger`    | `PositiveInteger`¹    |
| `xsd:string`             | `string`              |
| `xsd:unsignedInt`        | `uint`                |
| `xsd:unsignedShort`      | `ushort`              |

¹ `NonNegativeInteger` and `PositiveInteger` are `struct`s defined in this package as thin wrappers around
`System.Numeric.BigInteger` with suitable bounds checks at runtime.

Open vocabularies map to classes with one `public static readonly` field per member defined in the schema. To create a
nonstandard (`xsd:string`) value, use the public constructor.

```cs
// vocabulary:CharacterEncodingVocab is defined as (equivalent to either an xsd:string or) one of
//     "ASCII"^^vocabulary:CharacterEncodingVocab
//     "UTF-16"^^vocabulary:CharacterEncodingVocab
//     "UTF-32"^^vocabulary:CharacterEncodingVocab
//     "UTF-8"^^vocabulary:CharacterEncodingVocab
//     "Windows-1250"^^vocabulary:CharacterEncodingVocab
//     ...and so on...
CharacterEncodingVocab ascii = CharacterEncodingVocab.ASCII; // "ASCII"^^vocabulary:CharacterEncodingVocab
CharacterEncodingVocab custom = new CharacterEncodingVocab("Java-MUTF-8"); // "Java-MUTF-8"^^xsd:string

// The constructor doesn't check if you pass a lexical value that matches
// one of the well-known values. That is, this produces the literal node
// "ASCII"^^xsd:string, NOT "ASCII"^^vocabulary:CharacterEncodingVocab.
CharacterEncodingVocab likelyBug = new CharacterEncodingVocab("ASCII");
```

The generated graph is available through the `Graph` property on your `DataSet` and can be serialized using any of
`VDS.RDF`'s serialization mechanisms:

```cs
var rdfGraph = ds.Graph;
new VDS.RDF.Writing.CompressingTurtleWriter().Save(rdfGraph, myIoWriter);
```

## Known limitations

**Properties with no declared target type** are omitted from the bindings. As of version 1.0.0, this only affects the
following properties.

| RDF class                                   | Property path                            |
|---------------------------------------------|------------------------------------------|
| `action:ActionLifecycle`                    | `action:ActionStatus`                    |
| `observable:ExtractedString`                | `observable:byteStringValue`             |
| `observable:TCPConnectionFacet`             | `observable:destinationFlags`            |
| `observable:WirelessNetworkConnectionFacet` | `observable:wirelessNetworkSecurityMode` |
| `observable:WirelessNetworkConnectionFacet` | `observable:wirelessNetworkSecurityMode` |

**The `Thread` class** is intentionally unconstructable in this version of the bindings. It maps to the RDF class
`types:Thread`, which is not a bag of properties in the sense of the rest of the classes in the schema, but rather a
`subClassOf co:Bag` with unique semantics.
