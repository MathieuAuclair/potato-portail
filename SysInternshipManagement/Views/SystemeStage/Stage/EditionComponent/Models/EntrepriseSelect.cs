using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.Stage.EditionComponent.Models
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