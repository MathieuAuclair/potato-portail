using ApplicationPlanCadre.Models.ReunionsViewModel;

namespace PotatoPortail.Data
{
    public class CreateRepository
    {
        
        public CreerOrdreDuJourViewModel CreateLieu()
        {
            var lieuRepo = new LieuRepository();

            var allLieu = new CreerOrdreDuJourViewModel()
            {
                listLieux = lieuRepo.getLieu()
            };

            return allLieu;
        }
    }
}