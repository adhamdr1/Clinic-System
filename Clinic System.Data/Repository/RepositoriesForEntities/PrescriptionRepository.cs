namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
