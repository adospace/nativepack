using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace NativePack.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void ClassWithoutAttributes()
        {
            var generator = new Generator(GeneratorOptions.Empty);

            var generatedCode = generator.GenerateSerializerCode(Resources.Files.Class1());

            Assert.IsNull(generatedCode);
        }

        [TestMethod]
        public void ClassWithAttributesNoProperties()
        {
            var generator = new Generator(GeneratorOptions.Empty);

            var generatedCode = generator.GenerateSerializerCode(Resources.Files.Class2());

            Assert.IsNotNull(generatedCode);
        }


        [TestMethod]
        public void ClassWithProperties()
        {
            var generator = new Generator(GeneratorOptions.Empty);

            var generatedCode = generator.GenerateSerializerCode(Resources.Files.Class3());

            Assert.IsNotNull(generatedCode);
        }


        [TestMethod]
        public async Task EvaluateSerializer()
        {
            var generator = new Generator(GeneratorOptions.Empty);

            var sourceCode = Resources.Files.Class4();
            var generatedCode = generator.GenerateSerializerCode(Resources.Files.Class4());

            Assert.IsNotNull(generatedCode);

            bool res = false;
            try
            {
                res = await CSharpScript.EvaluateAsync<bool>(generatedCode + sourceCode + Resources.Files.Class4_TestCode(),
                    ScriptOptions.Default.WithReferences(typeof(System.Runtime.Serialization.DataContractAttribute).Assembly));
            }
            catch (Microsoft.CodeAnalysis.Scripting.CompilationErrorException e)
            {
                Debug.WriteLine(string.Join(Environment.NewLine, e.Diagnostics));
                Assert.Fail();
            }

            Assert.IsTrue(res);
        }
    }
}


