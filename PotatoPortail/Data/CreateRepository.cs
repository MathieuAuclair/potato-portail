
namespace PotatoPortail.Data
{
    public class CreateRepository
    {
        
        public CreerOrdreDuJourViewModel CreateLieu()
        {
            var lieuRepo = new LieuRepository();

            var allLieu = new CreerOrdreDuJourViewModel()
            {
                listLieux = lieuRepo.GetLieu()
            };

            return allLieu;
        }
    }
}