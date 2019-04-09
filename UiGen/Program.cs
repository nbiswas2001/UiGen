using System;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {

            var rootFolder = @"C:\Dev\Code\UiGen\Defn";
            var reader = new DefinitionReader();
            var def = reader.ReadDefnFromFile(rootFolder+ @"\app.component.txt");


            var generator = new Generator();
            var ctx = new GeneratorContext();
            ctx.outPath = rootFolder+ @"\sample-ui\src\app";
            ctx.fileExt = ".html";
            ctx.globalStyles = reader.LoadGlobalStylesFromFile("global-styles.txt");

            reader.LoadTemplatesFromFile("templates.txt", ctx.templatesMap);
            generator.ProcessDefn(def, ctx);

            Console.WriteLine("Done!");
        }


    }
}
