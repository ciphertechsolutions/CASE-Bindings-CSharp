using System.Collections.Generic;
using System.Linq;

namespace CT.CASE.Generator.Types
{
    public sealed class Class
    {
        public Iri Iri { get; private set; }

        public Iri SubclassOf { get; private set; }

        public string Comment { get; private set; }

        private Field[] _Fields;
        public IEnumerable<Field> Fields => _Fields;

        internal Class(Iri iri, Iri subclassOf, string comment, IEnumerable<Field> fields)
        {
            Iri = iri;
            SubclassOf = subclassOf;
            Comment = comment;
            _Fields = fields.ToArray();
        }

        public string ParentClassIdentifier => SubclassOf?.Suffix?.ToIdentifier() ?? "Thing";
    }
}
