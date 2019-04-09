using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UiGen
{
    class Generator
    {

        public void ProcessDefn(Definition defn, GeneratorContext ctx)
        {
            defn.rootContainer.Print();

            //Load elements into a map
            if(defn.contents!=null) foreach(var c in defn.contents) ctx.contentsMap.Add(c.id, c);
            if (defn.containers != null) foreach (var c in defn.containers) ctx.containerMap.Add(c.id, c);

            //Attach elements to the content containers
            var text = defn.rootContainer.Render(ctx);

            //remove blank lines
            text = text.Replace("\n\n", "\n");

            System.IO.File.WriteAllText(ctx.outPath+"\\"+defn.name+ctx.fileExt, text);

            //Write test file
            var testFilename = ctx.outPath + "\\" + defn.name + ".test" + ctx.fileExt;
            WriteTestFile(text, testFilename);

        }

        //-----------------------------------------------------------
        private void WriteTestFile(String text, String filename)
        {
            var testLines = new List<string>();
            string s1 = @"
            <!doctype html>
            <html lang=""en"">
              <head>
                <meta charset=""utf-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
                <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">
                <title>Test</title>
              </head>
              <body>";
            testLines.Add(s1);
            testLines.Add(text);
            testLines.Add("</body></html>");
            System.IO.File.WriteAllLines(filename, testLines);
        }

}

    //==============================
    class GeneratorContext
    {
        public string outPath;
        public string fileExt;
        public Dictionary<string, Template> templatesMap = new Dictionary<string, Template>();
        public Dictionary<string, ContentDefn> contentsMap = new Dictionary<string, ContentDefn>();
        public Dictionary<string, ContainerDefn> containerMap = new Dictionary<string, ContainerDefn>();
        public GlobalStyles globalStyles;
    }
}
