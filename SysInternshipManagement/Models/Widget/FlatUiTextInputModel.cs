namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiTextInputModel
    {
        public string Placeholder { get; set; }
        public string Height { get; set; }

        public FlatUiTextInputModel(string placeholder, string height)
        {
            Placeholder = placeholder;
            Height = height;
        }
        
        public FlatUiTextInputModel(string placeholder)
        {
            Placeholder = placeholder;
            Height = "auto";
        }
    }
}