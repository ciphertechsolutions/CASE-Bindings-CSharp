using System;
using System.Collections.Generic;
using System.Linq;

namespace CT.CASE.Generator.Types
{
    public sealed class Field
    {
        public Iri Path { get; private set; }

        public Iri Type { get; private set; }

        public Cardinality Cardinality { get; private set; }

        private static readonly Dictionary<string, string> XSD_TYPE_EXPRESSIONS = new Dictionary<string, string>
        {
            ["string"] = "string",
            ["unsignedInt"] = "uint",
            ["unsignedShort"] = "ushort",
            ["dateTime"] = "System.DateTime",
            ["nonNegativeInteger"] = "NonNegativeInteger",
            ["boolean"] = "bool",
            ["integer"] = "int",
            ["hexBinary"] = "byte[]",
            ["decimal"] = "decimal",
            ["anyURI"] = "System.Uri",
            ["byte"] = "sbyte",
            ["duration"] = "System.TimeSpan",
            ["positiveInteger"] = "PositiveInteger",
        };

        internal Field(Iri path, Iri type, Cardinality cardinality)
        {
            Path = path;
            Type = type;
            Cardinality = cardinality;
        }

        public string CsType()
        {
            var typeExpr = BaseTypeExpression();
            switch (Cardinality)
            {
                case Cardinality.One: break;
                case Cardinality.ZeroOrOne:
                    {
                        if (Type.IsPrimitive())
                        {
                            typeExpr += "?";
                        }
                        break;
                    }
                case Cardinality.ZeroOrMore:
                    {
                        typeExpr = $"System.Collections.Generic.IEnumerable<{typeExpr}>";
                        break;
                    }
            }
            return typeExpr;
        }

        /// <summary>
        /// Constructs a type expression representing a single element of this multi-valued field.
        /// Throws an exception if this field is not multi-valued.
        /// </summary>
        internal string ElementType()
        {
            switch (Cardinality)
            {
                case Cardinality.ZeroOrMore:
                    return BaseTypeExpression();
                default:
                    throw new InvalidOperationException($"{Cardinality}-cardinality does not have a well-defined element type.");
            }
        }

        private string BaseTypeExpression()
        {
            if (Type.Prefix != Iri.XSD_PREFIX)
            {
                return Type.Suffix.ToIdentifier();
            }
            if (XSD_TYPE_EXPRESSIONS.TryGetValue(Type.Suffix, out var typeExpression))
            {
                return typeExpression;
            }
            throw new NotSupportedException("XSD type " + Type);
        }

        public string LocalIdentifier()
        {
            return "@" + Path.Suffix.ToIdentifier().ToCamelCase();
        }

        public string FieldIdentifier()
        {
            return Path.Suffix.ToIdentifier().ToPascalCase();
        }

        /// <summary>
        /// Should we throw a NullArgumentException if someone passes <c>null</c> as an argument for this field?
        /// </summary>
        public bool NeedsNullCheck()
        {
            // Only if it's an exactly-one field with a reference type.
            return (Cardinality == Cardinality.One && !Type.IsPrimitive());
        }

        public bool NeedsEnumeration()
        {
            return (Cardinality == Cardinality.ZeroOrMore);
        }

        /// <summary>
        /// Is it syntactically possible for a C# variable of this field's declared type to contain <c>null</c>?
        /// </summary>
        public bool IsNullable()
        {
            // Yes, as long as it's not an exactly-one primitive field.
            return !(Cardinality == Cardinality.One && Type.IsPrimitive());
        }

        /// <summary>
        /// Should factory methods provide an `= null` default value for the parameters matching fields of this type?
        /// </summary>
        public bool HasParamDefault => Cardinality == Cardinality.ZeroOrOne || Cardinality == Cardinality.ZeroOrMore;
    }
}
