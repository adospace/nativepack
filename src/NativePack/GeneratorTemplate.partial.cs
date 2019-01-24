using NativePack.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack
{
    public partial class GeneratorTemplate
    {
        public GeneratorTemplate(IReadOnlyList<ClassDefinition> classDefinitions)
        {
            ClassDefinitions = classDefinitions;
        }

        public IReadOnlyList<ClassDefinition> ClassDefinitions { get; }

        private string GenerateWriterForProperty(PropertyDefinition propertyDefinition)
        {
            switch (propertyDefinition)
            {
                case ValueTypeMemberDefintion p:
                    if (!p.Nullable)
                        return $"writer.Write({p.Name});";
                    else
                        return $"writer.Write({p.Name} != null ? (byte)1 : (byte)0);" + Environment.NewLine +
                            $"            if ({p.Name} != null)" + Environment.NewLine +
                            $"                writer.Write({p.Name});";

                default:
                    throw new NotSupportedException();
            }
        }

        private string GenerateReaderForProperty(PropertyDefinition propertyDefinition)
        {
            switch (propertyDefinition)
            {
                case ValueTypeMemberDefintion p:
                    if (!p.Nullable)
                        return $"{p.Name} = reader.Read{p.Type}();";
                    else
                        return $"if (reader.ReadByte() == (byte)1)" + Environment.NewLine +
                            $"                {p.Name} = reader.Read{p.Type}();";

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
