using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativePack.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
namespace NativePack.Tests
{
    [TestClass]
    public class RuntimeTests
    {
        [Serializable]
        public class A
        {
            public int Id { get; set; }

            public string Message { get; set; }

            public B RefToB { get; set; }

            public B RefToC { get; set; } = new C();

            public Itf_D RefToD { get; set; }

            //public IList<string> ListOfStrings { get; set; }

            //public IList<B> ListOfB { get; set; }
        }

        [Serializable]
        public class B
        {
            public int? IdOfB { get; set; }
        }

        public enum SampleEnum
        {
            Enum1,

            Enum2,

            Enum3,
        }

        [Serializable]
        public class C : B
        {
            public C()
            {
                MyCustomEnum2 = SampleEnum.Enum3;
            }
            public SampleEnum? MyCustomEnum { get; set; } = SampleEnum.Enum2;

            public SampleEnum MyCustomEnum2 { get; set; }
        }

        public interface Itf_D
        {
            int PropertyOfD { get; }
        }

        [Serializable]
        public class D : Itf_D
        {
            public int PropertyOfD { get; set; }
        }

        [TestMethod]
        public void SimpleObject()
        {
            var serializer = new BinarySerializer();
            var ms = new MemoryStream();
            serializer.Serialize(new A()
            {
                Id = 1,
                Message = "test",
                RefToB = new B()
                {
                    IdOfB = 10
                },
                RefToD = new D()
                {
                    PropertyOfD = 12
                }
            }, ms);

            ms.Seek(0, SeekOrigin.Begin);
            var a = serializer.Deserialize<A>(ms);

            Assert.AreEqual(1, a.Id);
            Assert.AreEqual("test", a.Message);
            Assert.AreEqual(10, a.RefToB.IdOfB);
            Assert.AreEqual(null, a.RefToC.IdOfB);
            Assert.AreEqual(SampleEnum.Enum2, ((C)a.RefToC).MyCustomEnum);
            Assert.AreEqual(SampleEnum.Enum3, ((C)a.RefToC).MyCustomEnum2);
            Assert.AreEqual(12, a.RefToD.PropertyOfD);
        }
    }
}
