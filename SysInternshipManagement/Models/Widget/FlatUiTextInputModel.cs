namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiTextInputModel
    {
        public string Placeholder { get; set; }
        public string Height { get; set; }
        public string Name { get; set; }

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
    }
}