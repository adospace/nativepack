using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NativePack.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NativePack
{
    internal class Generator
    {
        private readonly GeneratorOptions _options;

        public Generator(GeneratorOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        internal void Run()
        {
            var filesToParse = GetFilesToParse();

            foreach (var fileToParse in filesToParse)
            {
                Debug($"Parsing {fileToParse}");
                var fileContent = File.ReadAllText(fileToParse);
                GenerateSerializerCode(fileContent);
            }
        }

        #region Tracing
        private void Trace(string level, string message, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"{level} {message}");
            Console.ForegroundColor = defaultColor;
        }
        private void Trace(string level, string message)
        {
            Console.WriteLine($"{level} {message}");
        }
        private void Debug(string message)
        {
            if (_options.Verbose)
            {
                Console.WriteLine($"DBG {message}");
            }
        }
        private void Warn(string message) => Trace("WRN", message, ConsoleColor.Magenta);

        private void Error(string message) => Trace("ERR", message, ConsoleColor.Red);
        #endregion

        internal string GenerateSerializerCode(string fileContent)
        {
            var fileTree = CSharpSyntaxTree.ParseText(fileContent);

            var compilation = (CompilationUnitSyntax)fileTree.GetRoot();

            if (compilation.ContainsDiagnostics)
            {
                if (compilation.GetDiagnostics().Any(_ => _.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error))
                {
                    Warn($"File contains some syntax errors that prevents analysis.");
                    return null;
                }
            }

            var listOfClasses = compilation
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(_ => _.IsSerializable());

            if (listOfClasses.Any())
            {
                var listOfClassDefinitions = new List<ClassDefinition>();
                Debug($"Found {listOfClasses.Count()}");

                foreach (var classDeclaration in listOfClasses)
                {
                    if (!classDeclaration.Modifiers.Any(_ => _.Text == "partial"))
                    {
                        Warn($"Class '{classDeclaration.Identifier.Text}' is marked with the Serializable attributes but is not partial: generation is skipped for this class");
                        continue;
                    }

                    var classDefinition = new ClassDefinition(
                        classDeclaration.Identifier.Text,
                        string.Join(" ", classDeclaration.Modifiers.Select(_=>_.Text)),
                        classDeclaration.ContainingNamespace(),
                        includeTypeName: classDeclaration.IncludeTypeName(),
                        callBaseSerializer: classDeclaration.CallBaseSerializer()
                        );

                    var properties = classDeclaration.DescendantNodes()
                        .OfType<PropertyDeclarationSyntax>()
                        .Where(_ => _.IsSerializable());

                    foreach (var property in properties)
                    {
                        if (property.IsEnum())
                        {
                            if (EnumPropertyDefinition.TryParse(property.Identifier.Text, property.Type.ToString(), out var defintion))
                                classDefinition.Members.Add(defintion);
                            else 
                                throw new InvalidOperationException("");
                        }
                        else
                        {
                            if (ValueTypeMemberDefinition.TryParse(property, out var valueTypeMemberDefinition))
                            {
                                classDefinition.Members.Add(valueTypeMemberDefinition);
                            }
                            else if (ListMemberDefinition.TryParse(property, out var listMemberDefinition))
                            {
                                classDefinition.Members.Add(listMemberDefinition);
                            }
                            else
                                throw new InvalidOperationException("");
                        }
                    }

                    var fields = classDeclaration.DescendantNodes()
                        .OfType<FieldDeclarationSyntax>()
                        .Where(_ => _.IsSerializable());

                    foreach (var field in fields)
                    {
                        foreach (var variable in field.Declaration.Variables)
                        {
                            if (ValueTypeMemberDefinition.TryParse(variable.Identifier.Text, field.Declaration.Type.ToString(), out var defintion))
                                classDefinition.Members.Add(defintion);
                            else
                                throw new InvalidOperationException("");
                        }
                    }

                    listOfClassDefinitions.Add(classDefinition);
                }

                if (listOfClassDefinitions.Any())
                {
                    var template = new GeneratorTemplate(listOfClassDefinitions);
                    return template.TransformText();
                }

            }

            return null;
        }

        internal IEnumerable<string> GetFilesToParse()
        {
            if (_options.Input == null)
                _options.Input = AppContext.BaseDirectory;

            var paths = _options.Input.Split(';');

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    //is a folder...
                    foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
                        yield return file;
                }
                else
                    yield return path;
            }
        }

    }
}
