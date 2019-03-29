using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Container
    {
        public List<Container> children = new List<Container>();
        public List<String> contentIds;
        public ContainerType type = ContainerType.Row;
        public List<ColWidthMarker> colWidthMarkers;
        public Container parent;
        public int columnWidth = -1;

        //-----------------------------------
        public void ResolveColumnWidth(int endCharPos)
        {
            if (parent.type == ContainerType.Row && parent.colWidthMarkers != null)
            {
                foreach (var cwm in parent.colWidthMarkers)
                {
                    if (cwm.pos == endCharPos)
                    {
                        this.columnWidth = cwm.width;
                        break;
                    }
                }
            }
        }

        //-------------------------------------
        public Container CreateChild(int scanBy)
        {
            var c = new Container();
            switch(scanBy)
            {
                case LayoutProcessor.ROW: c.type = ContainerType.Row; break;
                case LayoutProcessor.COL: c.type = ContainerType.Column; break;
                case -1: c.type = ContainerType.Content; break;
            }
            c.parent = this;
            this.children.Add(c);
            return c;
        }

        //-------------------------------------------------------------------
        public string Render(GeneratorContext ctx)
        {
            var result = "";
            var contentStringBuilder = new StringBuilder();
            if (type == ContainerType.Content)
            {
                if (contentIds == null) return "";

                foreach(var contentId in contentIds)
                {
                    if (ctx.contentsMap.ContainsKey(contentId))
                    {
                        var cDef = ctx.contentsMap[contentId];
                        contentStringBuilder.Append(cDef.Render(ctx));
                    }
                    else throw new Exception("Content with id '" + contentId + "' appears in the layout but is not defined");
                }
                result = contentStringBuilder.ToString();
            }
            else 
            {
                if(type == ContainerType.Row)
                {
                    //calculate the width of the last column
                    var totalWidth = 0;
                    var colCount = children.Count;
                    var widthPresent = false;
                    for(int i = 0; i < colCount; i++)
                    {
                        var ch = children[i];
                        if (ch.columnWidth != -1) totalWidth += ch.columnWidth;
                        if (i == 0)
                        {
                            widthPresent = ch.columnWidth != -1; //set if width is present on first col
                        }
                        else if(i > 0 && i < colCount - 1) //if middle row
                        {
                            if ((ch.columnWidth != -1) != widthPresent) //if widthPresent doesn't match error out
                                throw new Exception("Either all rows should have width or none at all.");
                        }
                        else //if last row
                        {
                            //if width was present then set width on last column
                            if (totalWidth > 0) ch.columnWidth = 12 - totalWidth;
                        }
                    }
                }

                //Load template args
                var n = type.ToString().ToLower();
                var hash = Hash.FromDictionary(ctx.layoutMap[n].data);

                //Render the content for children and add it to template args
                var cSB = new StringBuilder();
                foreach (var c in children) cSB.Append(c.Render(ctx));
                hash.Add("content", cSB.ToString());

                //If column add width as a template arg
                if (type == ContainerType.Column)
                {
                    var w = columnWidth > 0 ? "-" + columnWidth : "";
                    hash.Add("width", w);
                }

                //Get template and render
                var tmplt = ctx.templatesMap[n];
                result = tmplt.Render(hash);
            }

            return result;
        }

        //------------------------------------------------
        public void Print()
        {
            Console.WriteLine("\n***************\n");
            PrintRecurse(this, "");
            Console.WriteLine("\n");
        }

        private void PrintRecurse(Container cont, String indent)
        {
            if (cont.type == ContainerType.Content) Console.WriteLine(indent + String.Join(",", cont.contentIds));
            else
            {
                Console.WriteLine(indent + "C[" + cont.type + "]");
                foreach (var child in cont.children) PrintRecurse(child, indent + "  ");
            }
        }

    }

    //=====================
    enum ContainerType
    {
        Root, Row, Column, Content
    }

    //=======================
    struct ColWidthMarker
    {
        public ColWidthMarker(int width, int pos)
        {
            this.width = width;
            this.pos = pos;
        }
        public int width;
        public int pos;
    }
}
