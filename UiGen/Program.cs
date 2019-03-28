using System;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootFolder = @"C:\Dev\Code\MyProjects\NET\UiGen\Defn";
            var reader = new DefinitionReader();
            var def = reader.ReadDefnFromFile(rootFolder+@"\b.txt");

            var generator = new Generator();
            var ctx = new GeneratorContext();
            ctx.outPath = rootFolder+@"\output";
            ctx.fileExt = ".html";
            reader.LoadTemplatesFromFile("templates.txt", ctx.templatesMap);
            generator.ProcessDefn(def, ctx);

            Console.WriteLine("Done!");
        }


    }
}
