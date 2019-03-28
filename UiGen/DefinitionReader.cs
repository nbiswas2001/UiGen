using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace UiGen
{
    class DefinitionReader
    {
        public Definition ReadDefnFromFile(String filename)
        {
            //Read file
            string text = System.IO.File.ReadAllText(filename);

            //Convert to array of strings
            var lines = text.Split(Environment.NewLine);
            var defnTxt = GetDefinitionTexts(lines);
            var defn = ParseYaml(defnTxt.Item2);

            var layout = new LayoutProcessor();
            defn.rootContainer = layout.process(GetGrid(defnTxt.Item1));

            return defn;
        }

        //------------------------------------------
        private char[,] GetGrid(List<String> lines)
        {
            var lineCount = lines.Count;

            //Standard line length
            var lineLen = lines[0].Trim().Length;

            //The whole thing needs to be in a NxM character grid
            var grid = new char[lineCount, lineLen];

            //For each line ...
            for (var i = 0; i < lineCount; i++)
            {
                //Make further checks 
                var line = lines[i].Trim();
                if (line.Contains('\t')) throw new Exception("Line " + (i + 1) + " has tabs. Only use spaces.");
                if (line.Length != lineLen) throw new Exception("Line " + (i + 1) + " has incorrect length.");
                if (!line.StartsWith('-')) //If not an outer horizontal boundary line
                {
                    if (line[0] != '|' && line[line.Length - 1] != '|') throw new Exception("Line " + (i + 1) + " must start and end with '|'.");
                }

                //Put it in the grid
                for (var j = 0; j < lineLen; j++) grid[i, j] = line[j];
            }

            return grid;
        }

        //---------------------------------------
        private (List<string>, string) GetDefinitionTexts(string [] allLines)
        {
            const string SEP = "###";

            var layoutLines = new List<String>();
            var yml = new StringBuilder();
            bool inLayout = true;
            foreach(var line in allLines)
            {
                if (line == SEP) inLayout = false;

                if (inLayout)
                {
                    var l = line.Trim();
                    if(l!="") layoutLines.Add(l);
                } 
                else if(line.Trim() != SEP) yml.Append(line).Append("\n");
            }

            var result = (layoutLines, yml.ToString());
            return result;
        }

        //-----------------------------------------
        private Definition ParseYaml(String yml)
        {
            var deserialiser = new Deserializer();
            var input = new StringReader(yml);
            var defn = deserialiser.Deserialize<Definition>(input);
            return defn;
        }

        //--------------------------------------------------------------------------
        public void LoadTemplatesFromFile(string file, Dictionary<String, Template> templatesMap)
        {
           
            const string SEP = "###";
            string text = System.IO.File.ReadAllText(file);
            var lines = text.Split(Environment.NewLine);
            StringBuilder tmpltSB = null;
            string key = "";
            foreach(var line in lines)
            {
                if (line.StartsWith(SEP))
                {
                    if (tmpltSB != null)
                    {
                        var tmpltTxt = tmpltSB.ToString();
                        var t = Template.Parse(tmpltTxt);
                        if (!templatesMap.ContainsKey(key)) templatesMap.Add(key, t);
                        else throw new Exception("Template with key '" + key + "' already loaded.");
                    }

                    key = line.Substring(3).Trim();
                    tmpltSB = new StringBuilder();
                }
                else
                {
                    var l = line.Trim();
                    {
                        tmpltSB.Append("\n").Append(l);
                    }
                }

            }
            if (tmpltSB != null)
            {
                var tmpltTxt = tmpltSB.ToString();
                var t = Template.Parse(tmpltTxt);
                templatesMap.Add(key, t);
            }

        }

        //----------------------------------------------------------------
        public static Dictionary<String, object> ReadData(String dataTxt)
        {
            var data = new Dictionary<string, object>();
            var s1 = dataTxt.Split(",");
            foreach (var item in s1)
            {
                var s2 = item.Split("=");
                var k = s2[0].Trim();
                var v = s2[1].Trim();
                data.Add(k, v);
            }
            return data;

        }

    }
}
