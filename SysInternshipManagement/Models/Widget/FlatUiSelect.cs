using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SysInternshipManagement.Models.Widget
{
    public class FlatUiSelect
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public List<FlatUiOption> Options { get; set; }

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