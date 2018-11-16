using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.SystemeStage.Etudiant.EditionComponent.Models
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