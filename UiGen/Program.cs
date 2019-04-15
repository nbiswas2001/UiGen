using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace UiGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string settingsTxt = File.ReadAllText("settings.yml");
            var deserialiser = new Deserializer();
            var settings = deserialiser.Deserialize<Settings>(settingsTxt);


            var reader = new DefinitionReader();
            var generator = new Generator();
            var ctx = new GeneratorContext();
            ctx.globalStyles = reader.LoadGlobalStylesFromFile("global-styles.txt");
            reader.LoadTemplatesFromFile("global-templates.txt", ctx.templatesMap);
            reader.LoadTemplatesFromFile(settings.framework + "-templates.txt", ctx.templatesMap);


            var defFiles = new List<string>();
            LoadDefFiles(settings.rootDefinitionFolder, defFiles);


            foreach (var df in defFiles)
            {
                var def = reader.ReadDefnFromFile(df);
                if (def.outputFile == null)
                {
                    def.outputFile = df.Replace(settings.rootDefinitionFolder, settings.rootOutputFolder).Replace(".txt", settings.outputExt);
                }
                generator.ProcessDefn(def, ctx);
            }

            Console.WriteLine("Done!");
        }


        //---------------------------------------------------------
        static void LoadDefFiles(string sDir, List<string> files)
        {
            foreach (string f in Directory.GetFiles(sDir)) files.Add(f);
            foreach (string d in Directory.GetDirectories(sDir)) LoadDefFiles(d, files);
        }

    }
}
