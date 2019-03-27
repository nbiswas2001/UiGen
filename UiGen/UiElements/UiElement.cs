﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen.UiElements
{
    abstract class UiElement
    {
        public string id;
        public Dictionary<string, string> data;
        public abstract void Render();
    }
}