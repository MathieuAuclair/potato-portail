
using ApplicationPlanCadre.ViewModels.OrdresDuJourVM;

namespace PotatoPortail.Data
{
    public class CreateRepository
    {
        
        public OrdreDuJourViewModel CreateLieu()
        {
            var lieuRepo = new LieuRepository();

            var allLieu = new OrdreDuJourViewModel()
            {
                listLieux = lieuRepo.GetLieu()
            };

            return allLieu;
        }
    }
}