using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.SystemeStage.Application.ApplicationComponent.Models
{
    public class StateEntrepriseInformation
    {
        private readonly DatabaseContext _bd = new DatabaseContext();
        private SysInternshipManagement.Models.SystemeStage.Stage _stage;
        private Contact _contact;

        public StateEntrepriseInformation(SysInternshipManagement.Models.SystemeStage.Stage stage, Contact contact)
        {
            _stage = stage;
            _contact = contact;
        }
    }
}