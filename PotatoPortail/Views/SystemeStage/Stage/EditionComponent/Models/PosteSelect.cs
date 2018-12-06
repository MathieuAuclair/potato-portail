using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.Views.Stage.EditionComponent.Models
{
    public class PosteSelect
    {
        public List<Poste> Postes { get; set; }
        private readonly BDPortail _bd = new BDPortail();

        public PosteSelect()
        {
            Postes = _bd.Poste.ToList();
        }
    }
}