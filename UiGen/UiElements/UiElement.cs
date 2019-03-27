using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen.UiElements
{
    abstract class Content
    {
        public string id;
        public Dictionary<string, object> data;
        public abstract string Render();
        protected static Template template;

        public Content(ContentDefn defn)
        {
            this.id = defn.id;
            this.data = defn.data;
        }
    }
}
