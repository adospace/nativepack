using CommandLine;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NativePack
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GeneratorOptions>(args)
               .WithParsed(o =>
               {
                   new Generator(o).Run();
               });
        }
    }
}
