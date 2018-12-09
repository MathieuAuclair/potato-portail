using Microsoft.Ajax.Utilities;
using PotatoPortail.Models;

namespace PotatoPortail.Views.Etudiant.EditionComponent.Models
{
    public class PreferenceEditor
    {
        public Preference Preference { get; set; }

        public PreferenceEditor(Preference preference)
        {
            Preference = preference;
        }
    }
}