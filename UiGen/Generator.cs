using System;
using System.Collections.Generic;
using System.Text;
using UiGen.UiElements;

namespace UiGen
{
    class Generator
    {
        public string outPath;

        public void ProcessDefn(Definition defn)
        {
            defn.rootContainer.Print();

            //Load elements into a map
            Dictionary<string, UiElement> elems = new Dictionary<string, UiElement>();
            foreach(var e in defn.elements)
            {
                UiElement uie = null;
                switch (e.type)
                {
                    case "field": uie = new Field(e); break;
                }
                elems.Add(uie.id, uie);
            }
            ResolveUiElementRecurse(defn.rootContainer, elems);

        }

        //-------------------------------------------------
        private void ResolveUiElementRecurse(Container cont, 
                                             Dictionary<string, UiElement> elems)
        {
            if(cont.type == ContainerType.Content)
            {
                var id = cont.contentId;
                if(id != null && id != ""){
                    if (elems.ContainsKey(id)) cont.uiElement = elems[id];
                    else throw new Exception("UiElement with id '" + id + "' not found");
                }
            }
            else
            {
                foreach(var c in cont.children) ResolveUiElementRecurse(c, elems);
            }
        }
    }
}
