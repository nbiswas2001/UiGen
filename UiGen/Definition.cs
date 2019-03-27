using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen
{
    class Definition
    {
        public char[,] grid;

        public LayoutDefn layout;

        public List<ContentDefn> content;
    }

    class ContentDefn
    {
        public string id;
        public string type;
    }

    class LayoutDefn
    {

    }
}

