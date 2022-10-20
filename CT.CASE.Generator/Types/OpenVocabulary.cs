using System.Collections.Generic;
using System.Linq;

namespace CT.CASE.Generator.Types
{
    public sealed class OpenVocabulary
    {
        public Iri Iri { get; private set; }

        private Member[] _Members;

        public IEnumerable<Member> Members => _Members;

        internal OpenVocabulary(Iri iri, IEnumerable<Member> members)
        {
            Iri = iri;
            _Members = members.ToArray();
        }

        public sealed class Member
        {
            public string LexicalValue { get; private set; }

            public string Identifier => LexicalValue.ToIdentifier();

            internal Member(string lexical)
            {
                LexicalValue = lexical;
            }
        }
    }
}
