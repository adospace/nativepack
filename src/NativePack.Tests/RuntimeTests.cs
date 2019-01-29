using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativePack.Runtime;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
namespace NativePack.Tests
{
    [Serializable]
    public class A
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public B RefToB { get; set; }
    }

    [Serializable]
    public class B
    {
        public int? IdOfB { get; set; }
    }

    [TestClass]
    public class RuntimeTests
    {
        [TestMethod]
        public void SimpleObject()
        {
            var serializer = new BinarySerializer();
            var ms = new MemoryStream();
            serializer.Serialize(new A() { Id = 1, Message = "test", RefToB = new B() { IdOfB = 10 } }, ms);

            ms.Seek(0, SeekOrigin.Begin);
            var a = serializer.Deserialize<A>(ms);

            Assert.AreEqual(1, a.Id);
            Assert.AreEqual("test", a.Message);
            Assert.AreEqual(10, a.RefToB.IdOfB);
        }
    }
}
