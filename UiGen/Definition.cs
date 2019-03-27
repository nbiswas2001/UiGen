using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public Container rootContainer;
        public string name;
        public LayoutDefn layout;
        public List<ContentDefn> contents;
    }

    //============================
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
                if (_data == null)
                {
                    _data = new Dictionary<string, object>();
                    var s1 = defn.Split(",");
                    foreach (var item in s1)
                    {
                        var s2 = item.Split("=");
                        var k = s2[0].Trim();
                        var v = s2[1].Trim();
                        //Strip '' from around strings
                        if(v.StartsWith("'") && v.EndsWith("'"))
                        {
                            var strVal = v.Substring(1, v.Length - 2).Trim();
                            _data.Add(k, strVal);
                        }
                        else
                        {
                            var isNumeric = int.TryParse(v, out int intVal);
                            if (isNumeric)
                            {
                                _data.Add(k, intVal);
                            }
                            else
                            {
                                var isBool = bool.TryParse(v, out bool boolVal);
                                if (isBool)
                                {
                                    _data.Add(k, boolVal);
                                }
                            }
                        }
                        
                    }
                }
                return _data;
            }
        }
    }

    //======================
    class LayoutDefn
    {

    }
}

