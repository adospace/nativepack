using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativePack
{
    internal class GeneratorOptions
    {
        [Option('i', "input", Required = false, HelpText = "Specify input of the analyzer: can be one or more files (separated by ';') or a folder. Default is current folder.")]
        public string Input { get; set; }

        [Option('o', "output", Required = false, HelpText = "Specify an output pattern for generated files. Default('{0}.serializer.cs).')")]
        public string Output { get; set; } = "{0}.serializer.cs";

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        public static GeneratorOptions Empty { get; } = new GeneratorOptions();
    }
}
