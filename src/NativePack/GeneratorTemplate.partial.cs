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

        //private string GenerateWriterForProperty(PropertyDefinition propertyDefinition)
        //{
        //    switch (propertyDefinition)
        //    {
        //        case ValueTypeMemberDefintion p:
        //            return p.GenerateSerializerCode();

        //        default:
        //            throw new NotSupportedException();
        //    }
        //}

        //private string GenerateReaderForProperty(PropertyDefinition propertyDefinition)
        //{
        //    switch (propertyDefinition)
        //    {
        //        case ValueTypeMemberDefintion p:

        //        default:
        //            throw new NotSupportedException();
        //    }
        //}
    }
}
