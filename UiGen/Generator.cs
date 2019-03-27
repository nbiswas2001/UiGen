using System;
using System.Collections.Generic;
using System.Text;
using UiGen.UiElements;

namespace UiGen
{
    class Generator
    {
        public string outPath;
        public string fileExt;

        public void ProcessDefn(Definition defn)
        {
            defn.rootContainer.Print();

            //Load elements into a map
            Dictionary<string, Content> contentsMap = new Dictionary<string, Content>();
            foreach(var cDef in defn.contents)
            {
                Content content = null;
                switch (cDef.type)
                {
                    case "field": content = new Field(cDef); break;
                }
                contentsMap.Add(content.id, content);
            }

            //Attach elements to the content containers
            ResolveUiElementRecurse(defn.rootContainer, contentsMap);


            var ctx = new GeneratorContext();
            ctx.outPath = outPath;
            ctx.fileExt = fileExt;
            defn.rootContainer.Render(ctx);
        }

        //-------------------------------------------------
        private void ResolveUiElementRecurse(Container cont, 
                                             Dictionary<string, Content> elems)
        {
            if(cont.type == ContainerType.Content)
            {
                var id = cont.contentId;
                if(id != null && id != ""){
                    if (elems.ContainsKey(id)) cont.content = elems[id];
                    else throw new Exception("UiElement with id '" + id + "' not found");
                }
            }
            else
            {
                foreach(var c in cont.children) ResolveUiElementRecurse(c, elems);
            }
        }
    }

    //==============================
    class GeneratorContext
    {
        public string outPath;
        public string fileExt;


    }
}
