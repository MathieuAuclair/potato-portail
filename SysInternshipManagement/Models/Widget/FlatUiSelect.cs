namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiSelect
    {
        public string Label { get; set; }
        public string Name { get; set; }

        public FlatUiSelect()
        {
        }

        public FlatUiSelect(string label)
        {
            Label = label;
        }

        public FlatUiSelect(string label,string name)
        {
            Label = label;
            Name = name;
        }
    }
}