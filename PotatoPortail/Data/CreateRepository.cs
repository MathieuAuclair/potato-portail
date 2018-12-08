
using PotatoPortail.ViewModels.OrdresDuJour;

namespace PotatoPortail.Data
{
    public class CreateRepository
    {
        
        public OrdreDuJourViewModel CreateLieu()
        {
            var lieuRepo = new LieuRepository();

            var allLieu = new OrdreDuJourViewModel()
            {
                ListLieux = lieuRepo.GetLieu()
            };

            return allLieu;
        }
    }
}