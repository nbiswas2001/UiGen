using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public Container rootContainer;
        public string outputFile;
        public string testFile;
        public List<ContainerDefn> containers;
        public List<ContentDefn> contents;
    }

    //======================================================
    class GlobalStyles
    {
        public GlobalStyles(Dictionary<string, string> map)
        {
            this.map = map;
        }
        private Dictionary<string, string> map;

        private Dictionary<string, Dictionary<string, object>> _stylesData;
        public Dictionary<string, Dictionary<string, object>> stylesData
        {
            get
            {
                if (_stylesData == null)
                {
                    _stylesData = new Dictionary<string, Dictionary<string, object>>();
                    foreach (var k in map.Keys)
                    {
                        var data = DefinitionReader.ReadData(map[k]);
                        _stylesData.Add(k, data);
                    }
                }
                return _stylesData;
            }
        }
    }

    //==========================================
    class ContainerDefn
    {
        public string id;
        public string defn;

        private Dictionary<String, object> _data;
        public Dictionary<String, object> data
        {
            get
            {
                if (_data == null) _data = DefinitionReader.ReadData(defn);
                return _data;
            }
        }
    }

    //========================================
    class ContentDefn
    {
        public string id;
        public string type;
        public string defn;

        private Dictionary<string, object> _data;
        public Dictionary<string, object> data
        {
            get
            {
                if (_data == null) _data = DefinitionReader.ReadData(defn);
                return _data;
            }
        }

        //---------------------------------------
        public string Render(GeneratorContext ctx)
        {
            var tmplt = ctx.templatesMap[type];
            var hash = Hash.FromDictionary(data);
            var result = tmplt.Render(hash);
            return result;
        }

    }
}

