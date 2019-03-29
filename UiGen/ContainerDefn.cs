using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
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

}
