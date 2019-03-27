using System;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {

            DefinitionReader reader = new DefinitionReader();
            var def = reader.ReadFromFile(@"C:\Dev\Code\MyProjects\NET\UiGen\Defn\a.txt");
            var layout = new LayoutProcessor();
            var root = layout.process(def.grid);
            root.Print();

            Console.Read();
        }


    }
}
