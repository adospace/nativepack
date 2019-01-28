using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack
{
    public partial class ListDeserializerTemplate
    {
        public ListDeserializerTemplate(Model.ListMemberDefinition list)
        {
            List = list;
            
        }

        public Model.ListMemberDefinition List { get; }
    }
}
