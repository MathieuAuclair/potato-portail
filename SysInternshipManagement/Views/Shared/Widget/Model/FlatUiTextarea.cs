namespace SysInternshipManagement.Views.Shared.Widget.Model
{
    public class FlatUiTextarea
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Valeur { get; set; }

        public FlatUiTextarea(string label,string name, string valeur)
        {
            Label = label;
            Name = name;
            Valeur = valeur;
        }
    }
}