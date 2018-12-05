namespace PotatoPortail.Views.Shared.Widget.Model
{
    public class FlatUiTextInputModel
    {
        public string Placeholder { get; set; }
        public string Height { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Valeur { get; set; }
        public string Step { get; set; }

        public FlatUiTextInputModel(string placeholder, string height, string name, string type, string valeur, float? step)
        {
            Placeholder =placeholder;
            Height = height ?? "auto";
            Name = name;
            Type = type ?? "text";
            Valeur = valeur;
            Step = step.ToString();
        }
    }
}