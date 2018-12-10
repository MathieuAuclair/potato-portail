using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.SystemeStage.Application.ApplicationComponent.Models
{
    public class StateEntrepriseInformation
    {
        private readonly BdPortail _bd = new BdPortail();
        private PotatoPortail.Models.Stage _stage;
        private Contact _contact;

        public StateEntrepriseInformation(PotatoPortail.Models.Stage stage, Contact contact)
        {
            _stage = stage;
            _contact = contact;
        }
    }
}