using System.Collections.Generic;
using System.Linq;
using PotatoPortail.Migrations;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class StructureViewModel
    {
        public Models.PlanCadre PlanCadre { get; set; }

        public int RecupererIdPlanCadreElement(int idPlanCadreCompetence, int idElement)
        {
            var db = new BdPortail();

            var idsPlanCadreElement = from planCadreElement in db.PlanCadreElement
                where planCadreElement.IdPlanCadreCompetence == idPlanCadreCompetence &&
                      planCadreElement.IdElement == idElement
                select planCadreElement.IdElement;

            var idPlanCadreElement = idsPlanCadreElement.First();

            return idPlanCadreElement;
        }
    }
}