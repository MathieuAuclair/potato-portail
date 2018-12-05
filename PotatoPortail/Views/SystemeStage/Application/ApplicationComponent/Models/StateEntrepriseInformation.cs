using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Application.ApplicationComponent.Models
{
    public class StateEntrepriseInformation
    {
        private readonly DatabaseContext _bd = new DatabaseContext();
        private PotatoPortail.Models.SystemeStage.Stage _stage;
        private Contact _contact;

        public StateEntrepriseInformation(PotatoPortail.Models.SystemeStage.Stage stage, Contact contact)
        {
            _stage = stage;
            _contact = contact;
        }
    }
}