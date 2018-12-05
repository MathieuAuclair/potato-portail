using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Etudiant.EditionComponent.Models
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