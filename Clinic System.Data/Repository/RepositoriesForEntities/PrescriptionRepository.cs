
namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Prescription?> GetPrescriptionWithDetailsAsync(int prescriptionId, CancellationToken cancellationToken)
        {
            return await context.Prescriptions
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr.Appointment)
                .FirstOrDefaultAsync(p => p.Id == prescriptionId, cancellationToken);
        }
    }
}
