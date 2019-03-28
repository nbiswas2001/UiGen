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

        private Dictionary<String, object> _data;
        public Dictionary<String, object> data
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

            if (type == "table")
            {
                var colTxt = (string) hash["columns"];
                hash["columns"] =GetTableColumnNames(colTxt);
            }

            var result = tmplt.Render(hash);
            return result;
        }

        //--------------------------------------------------
        private List<String> GetTableColumnNames(String colTxt)
        {
            var colsArr = colTxt.Split("|");
            var colNames = new List<String>();
            foreach (var col in colsArr) colNames.Add(col.Trim());
            return colNames;
        }
    }
}
