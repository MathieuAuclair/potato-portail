using System;

namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiTextInputModel
    {
        public string Placeholder { get; set; }
        public string Height { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public FlatUiTextInputModel(string placeholder, string height)
        {
            Placeholder = placeholder;
            Height = height;
        }

        public FlatUiTextInputModel(string placeholder, string height, string name)
        {
            Placeholder = placeholder;
            Height = height;
            Name = name;
        }

        public FlatUiTextInputModel(string placeholder)
        {
            Placeholder = placeholder;
            Height = "auto";
        }

        public FlatUiTextInputModel(string placeholder, string height, string name, string type, string value)
        {
            Placeholder =placeholder;
            Height = height ?? "auto";
            Name = name;
            Type = type ?? "text";
            Value = value;
        }
    }
}