using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public Container rootContainer;
        public string name;
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

}

