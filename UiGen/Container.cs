using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Container
    {
        public List<Container> children = new List<Container>();
        public String contentId;
        public ContainerType type = ContainerType.Row;
        public List<ColWidthMarker> colWidthMarkers;
        public Container parent;
        public int columnWidth = -1;

        //------------------------------------------------
        public void Print()
        {
            Console.WriteLine("\n***************\n");
            PrintRecurse(this, "");
            Console.WriteLine("\n");
        }

        private void PrintRecurse(Container cont, String indent)
        {
            if (cont.type == ContainerType.Content) Console.WriteLine(indent + cont.contentId);
            else
            {
                Console.WriteLine(indent + "C[" + cont.type + "]");
                foreach (var child in cont.children) PrintRecurse(child, indent + "  ");
            }
        }

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
