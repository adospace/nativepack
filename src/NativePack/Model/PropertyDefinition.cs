using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    public abstract class PropertyDefinition
    {
        public PropertyDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
