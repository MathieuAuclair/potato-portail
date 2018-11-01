using Microsoft.Ajax.Utilities;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Etudiant.EditionComponent.Models
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