using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public Container rootContainer;
        public LayoutDefn layout;
        public List<UiElementDefn> elements;
    }

    //============================
    class UiElementDefn
    {
        public string id;
        public string type;
        public string defn;

        private Dictionary<String, String> _data;
        public Dictionary<String, String> data
        {
            get
            {
                if (_data == null)
                {
                    _data = new Dictionary<string, string>();
                    var s1 = defn.Split(",");
                    foreach (var item in s1)
                    {
                        var s2 = item.Split("=");
                        var k = s2[0].Trim();
                        var v = s2[1].Trim();
                        //Strip '' from around strings
                        if(v.StartsWith("'") && v.EndsWith("'"))
                        {
                            v = v.Substring(1, v.Length - 2);
                        }
                        _data.Add(k, v);
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

