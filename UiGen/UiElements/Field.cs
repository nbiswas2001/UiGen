using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace UiGen.UiElements
{
    class Field : Content
    {
        public Field(ContentDefn defn) : base(defn) { }

        private static string templateTxt =
        @"
        <div class=""form-group"">
            <label>@Model.label</label>
            <input type = ""text"" class=""form-control"" name=""@Model.name"" [(ngModel)]=""@Model.source"" />
        </div>
        ";

        static Field()
        {
            template = Template.Parse(templateTxt);
        }

        public override string Render()
        {
            var rendered = template.Render(Hash.FromDictionary(data));
            return rendered;

        }
    }
}
