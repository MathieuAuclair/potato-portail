namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiTextarea
    {
        public string Label { get; set; }
        public string Name { get; set; }

        public FlatUiTextarea()
        {
        }

        public FlatUiTextarea(string label)
        {
            Label = label;
        }

        public FlatUiTextarea(string label,string name)
        {
            Label = label;
            Name = name;
        }
    }
}