using System;
using System.IO;
using CT.CASE.Generator.Types;

namespace CT.CASE.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: CT.CASE.Generator OUTFILE.cs");
                Environment.Exit(1);
            }

            var outputPath = args[0];

            var schema = Schema.Load();
            var template = new BindingsTemplate(schema);
            using (var output = File.CreateText(outputPath))
            {
                output.Write(template.TransformText());
            }
        }
    }
}
