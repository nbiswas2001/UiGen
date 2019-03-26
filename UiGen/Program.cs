using System;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read file
            string text = System.IO.File.ReadAllText(@"C:\Dev\Code\MyProjects\NET\UiGen\Defn\a.txt");

            //Convert to array of strings
            var lines = text.Split("\r\n");

            //number of lines
            var lineCount = lines.Length;

            //Standard line length
            var lineLen = lines[0].Trim().Length;

            //The whole thing needs to be in a NxM character grid
            var grid = new char[lineCount, lineLen];

            //For each line ...
            for (var i=0; i < lineCount; i++)
            {
                //Make further checks 
                var line = lines[i].Trim();
                if(line.Contains('\t')) throw new Exception("Line " + (i + 1) + " has tabs. Only use spaces.");
                if (line.Length != lineLen) throw new Exception("Line " + (i + 1) + " has incorrect length.");
                if (!line.StartsWith('-')) //If not an outer horizontal boundary line
                {
                    if(line[0] != '|' && line[line.Length-1] != '|') throw new Exception("Line " + (i + 1) + " must start and end with '|'.");
                }

                //Put it in the grid
                for (var j=0; j < lineLen; j++) grid[i, j] = line[j];
            }

            var layout = new Layout();
            layout.process(grid);
            layout.root.Print();

            Console.Read();
        }


    }
}
