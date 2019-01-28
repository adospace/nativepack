using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public class ClassDefinition
    {
        public ClassDefinition(string name, string modifier, 
            string ns = null, 
            bool includeTypeName = true, 
            bool callBaseSerializer = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("can't be empty or null", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(modifier))
            {
                throw new ArgumentException("message", nameof(modifier));
            }

            Namespace = ns;
            IncludeTypeName = includeTypeName;
            CallBaseSerializer = callBaseSerializer;
            Name = name;
            Modifier = modifier;
        }

        public string Namespace { get; }
        public bool IncludeTypeName { get; }
        public bool CallBaseSerializer { get; }

        public bool HasNamespace => Namespace != null;
        public string Name { get; }
        public string Modifier { get; }

        public List<MemberDefinition> Members { get; private set; } = new List<MemberDefinition>();
    }
}
