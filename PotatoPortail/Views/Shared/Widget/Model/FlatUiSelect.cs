using System.Collections.Generic;

namespace PotatoPortail.Views.Shared.Widget.Model
{
    public class FlatUiSelect
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public List<FlatUiOption> Options { get; set; }

        public FlatUiSelect(string label, string name, List<FlatUiOption> options)
        {
            Label = label;
            Name = name;
            Options = options;
        }
    }

    public class FlatUiOption
    {
        public string ContenuHtml { get; set; }
        public string Valeur { get; set; }

        public FlatUiOption(string contenuHtml, string valeur)
        {
            ContenuHtml = contenuHtml;
            Valeur = valeur;
        }
    } 
}