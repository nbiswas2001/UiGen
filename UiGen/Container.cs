using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Container
    {
        public List<Container> children = new List<Container>();
        public String content;
        public ContainerType type = ContainerType.Row;
        public List<int> colWidthMarkers = new List<int>();

        //------------------------------------------------
        public void Print()
        {
            Console.WriteLine("***************\n");
            PrintRec(this, "");
        }

        private void PrintRec(Container cont, String indent)
        {
            if (cont.type == ContainerType.Content)
            {
                Console.WriteLine(indent + cont.content);
            }
            else
            {
                Console.WriteLine(indent + "C[" + cont.type + "]");
                foreach (var child in cont.children) PrintRec(child, indent + "  ");
            }
        }

    }

    //=====================
    enum ContainerType
    {
        Root, Row, Column, Content
    }

}
