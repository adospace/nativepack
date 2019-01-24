using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NativePack.Tests.Resources
{
    [DataContract]
    public partial class Class3
    {

        [DataMember]
        public int PropertyInt1 { get; private set; }

        [DataMember]
        public int? PropertyInt2 { get; set; }

        [DataMember]
        int _fieldInt3 = 0;

        [DataMember]
        int? _fieldInt4 = 0;

        [DataMember]
        public bool Property1 { get; }
        [DataMember]
        public System.Boolean? Property2 { get; }

        [DataMember]
        public byte Property3 { get; }
        [DataMember]
        public System.Byte? Property4 { get; }

        [DataMember]
        public char Property5 { get; }
        [DataMember]
        public System.Char? Property6 { get; }

        [DataMember]

        public double Property7 { get; }
        [DataMember]
        public System.Double? Property8 { get; }

        [DataMember]
        public float Property9 { get; }
        [DataMember]
        public System.Single? Property10 { get; }

        [DataMember]
        public Int32 Property11 { get; }
        [DataMember]
        public System.Int32? Property12 { get; }

        [DataMember]
        public long Property13 { get; }
        [DataMember]
        public System.Int64? Property14 { get; }

        [DataMember]
        public sbyte Property15 { get; }
        [DataMember]
        public System.SByte? Property16 { get; }

        [DataMember]
        public short Property17 { get; }
        [DataMember]
        public System.Int16? Property18 { get; }

        [DataMember]
        public uint Property19 { get; }
        [DataMember]
        public System.UInt32? Property20 { get; }

        [DataMember]
        public ulong Property21 { get; }
        [DataMember]
        public System.UInt64? Property22 { get; }

        [DataMember]
        public ushort Property23 { get; }
        [DataMember]
        public System.UInt16? Property24 { get; }

        [DataMember]
        public decimal Property25 { get; }
        [DataMember]
        public System.Decimal? Property26 { get; }


    }

}

//Replace




