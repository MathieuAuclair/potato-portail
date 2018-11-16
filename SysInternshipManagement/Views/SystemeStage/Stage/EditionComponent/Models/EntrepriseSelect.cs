using System.Collections.Generic;
using System.Linq;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Views.Stage.EditionComponent.Models
{
    public class EntrepriseSelect
    {
        public List<Entreprise> Entreprises { get; set; }
        private readonly DatabaseContext _bd = new DatabaseContext();

        public EntrepriseSelect()
        {
            Entreprises = _bd.Entreprise.ToList();
        }
    }
}