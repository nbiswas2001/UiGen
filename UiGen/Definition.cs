using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public Container rootContainer;
        public string name;
        public List<LayoutDefn> layout;
        public List<ContentDefn> contents;

        //--------------------------------------
        public Dictionary<string, LayoutDefn> GetLayoutMap ()
        {
            var result = new Dictionary<string, LayoutDefn>();
            result.Add("root", GetLayout("root", "class=container"));
            result.Add("row", GetLayout("row", "class=row"));
            result.Add("column", GetLayout("column", "size=sm"));
            return result;
        }

        //-----------------------------------
        private LayoutDefn GetLayout(string type, string defaultDefn)
        {
            var lo = layout.Find(l => l.type == type);
            if (lo == null)
            {
                lo = new LayoutDefn();
                lo.defn = defaultDefn;
            }
            return lo;
        }
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
                if (_data == null) _data = DefinitionReader.ReadData(defn);
                return _data;
            }
        }
    }

    //======================
    class LayoutDefn
    {
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
    }
}

