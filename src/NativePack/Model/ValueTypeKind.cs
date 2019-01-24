using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack.Model
{
    /*
    bool. byte. char. decimal. double. enum. float. int. long. sbyte. short. struct. uint. ulong. ushort. 
    
        public virtual bool ReadBoolean();
        public virtual byte ReadByte();
        public virtual byte[] ReadBytes(int count);
        public virtual char ReadChar();
        public virtual char[] ReadChars(int count);
        public virtual decimal ReadDecimal();
        public virtual double ReadDouble();
        public virtual short ReadInt16();
        public virtual int ReadInt32();
        public virtual long ReadInt64();
        public virtual sbyte ReadSByte();
        public virtual float ReadSingle();
        public virtual string ReadString();
        public virtual ushort ReadUInt16();
        public virtual uint ReadUInt32();
        public virtual ulong ReadUInt64();

    */
    public enum ValueTypeKind
    {
        Undefined = 0,

        Boolean = 1,

        Byte = 2,

        Char = 3,

        Decimal = 4,

        Double = 5,

        Enum = 6,

        Int16 = 7,

        Int32 = 8,

        Int64 = 9,

        SByte = 10,

        Single = 11,

        UInt16 = 12,

        UInt32 = 13,

        UInt64 = 14,

        Struct = 15,
    }
}
