using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PotatoPortail.Migrations;
using PotatoPortail.Models;

namespace PotatoPortail.ViewModels.PlanCadre
{
    public class StructureViewModel
    {
        public Models.PlanCadre PlanCadre { get; set; }
        public ICollection<ElementEnoncePlanCadre> ElementEnoncePlanCadres { get; set; }

        public static int RecupererIdPlanCadreElement(int idPlanCadreCompetence, int idElement)
        {
            var db = new BdPortail();

            var idsPlanCadreElement = from planCadreElement in db.PlanCadreElement
                where planCadreElement.IdPlanCadreCompetence == idPlanCadreCompetence &&
                      planCadreElement.IdElement == idElement
                select planCadreElement.IdPlanCadreElement;

            var idPlanCadreElement = idsPlanCadreElement.First();

            return idPlanCadreElement;
        }
    }
}