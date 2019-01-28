using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NativePack.Tests.Resources
{
    [GenerateSerializer]
    public partial class Class3
    {

        [GenerateSerializer]
        public int PropertyInt1 { get; private set; }

        [GenerateSerializer]
        public int? PropertyInt2 { get; set; }

        [GenerateSerializer]
        int _fieldInt3 = 0;

        [GenerateSerializer]
        int? _fieldInt4 = 0;

        [GenerateSerializer]
        public bool Property1 { get; }
        [GenerateSerializer]
        public System.Boolean? Property2 { get; }

        [GenerateSerializer]
        public byte Property3 { get; }
        [GenerateSerializer]
        public System.Byte? Property4 { get; }

        [GenerateSerializer]
        public char Property5 { get; }
        [GenerateSerializer]
        public System.Char? Property6 { get; }

        [GenerateSerializer]

        public double Property7 { get; }
        [GenerateSerializer]
        public System.Double? Property8 { get; }

        [GenerateSerializer]
        public float Property9 { get; }
        [GenerateSerializer]
        public System.Single? Property10 { get; }

        [GenerateSerializer]
        public Int32 Property11 { get; }
        [GenerateSerializer]
        public System.Int32? Property12 { get; }

        [GenerateSerializer]
        public long Property13 { get; }
        [GenerateSerializer]
        public System.Int64? Property14 { get; }

        [GenerateSerializer]
        public sbyte Property15 { get; }
        [GenerateSerializer]
        public System.SByte? Property16 { get; }

        [GenerateSerializer]
        public short Property17 { get; }
        [GenerateSerializer]
        public System.Int16? Property18 { get; }

        [GenerateSerializer]
        public uint Property19 { get; }
        [GenerateSerializer]
        public System.UInt32? Property20 { get; }

        [GenerateSerializer]
        public ulong Property21 { get; }
        [GenerateSerializer]
        public System.UInt64? Property22 { get; }

        [GenerateSerializer]
        public ushort Property23 { get; }
        [GenerateSerializer]
        public System.UInt16? Property24 { get; }

        [GenerateSerializer]
        public decimal Property25 { get; }
        [GenerateSerializer]
        public System.Decimal? Property26 { get; }


    }

}

//Replace




