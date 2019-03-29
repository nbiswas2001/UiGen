using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
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
