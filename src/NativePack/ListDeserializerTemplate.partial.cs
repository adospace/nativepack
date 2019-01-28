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
            /*
bw.Write(<#= List.Name #>.Count);
foreach (var item in <#= List.Name #>)
{
    <# if (List.Type != NativePack.Model.ValueTypeKind.Undefined) { #>

    <# if (!List.Nullable) { #>
    writer.Write(item);
    <# } else {#>
    writer.Write(item != null ? (byte)1 : (byte)0);
    if (item != null)
        writer.Write(item);
    <# } #>

    <# } #>
}
            */
        }

        public Model.ListMemberDefinition List { get; }
    }
}
