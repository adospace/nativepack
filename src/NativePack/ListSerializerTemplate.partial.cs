using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack
{
    public partial class ListSerializerTemplate
    {
        public ListSerializerTemplate(Model.ListMemberDefinition list)
        {
            List = list;
            
        }

        public Model.ListMemberDefinition List { get; }
    }
}
