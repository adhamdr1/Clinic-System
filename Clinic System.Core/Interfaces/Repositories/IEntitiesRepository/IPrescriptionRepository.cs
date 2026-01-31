namespace Clinic_System.Core.Interfaces.Repositories.IEntitiesRepository
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        Task<Prescription?> GetPrescriptionWithDetailsAsync(int prescriptionId,CancellationToken cancellationToken);
    }
}