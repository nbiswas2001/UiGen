using System;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {

            var reader = new DefinitionReader();
            var def = reader.ReadDefnFromFile(@"C:\Dev\Code\MyProjects\NET\UiGen\Defn\a.txt");

            var generator = new Generator();
            generator.outPath = @"C:\Dev\Code\MyProjects\NET\UiGen\Defn";
            generator.fileExt = ".html";
            generator.ProcessDefn(def);

            Console.Read();
        }


    }
}
