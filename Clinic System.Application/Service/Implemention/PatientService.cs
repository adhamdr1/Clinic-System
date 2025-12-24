namespace Clinic_System.Application.Service.Implemention
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<Patient?>> GetPatientsListAsync(CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.PatientsRepository
                .GetAllAsync(cancellationToken: cancellationToken)).ToList();
        }

        public async Task<PagedResult<Patient?>> GetPatientsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.PatientsRepository.GetPaginatedAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            return new PagedResult<Patient>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<Patient?> GetPatientWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.PatientsRepository.GetPatientWithAppointmentsByIdAsync(id, cancellationToken);
        }
        public async Task CreatePatientAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            await unitOfWork.PatientsRepository.AddAsync(patient, cancellationToken);
        }

        public async Task UpdatePatient(Patient patient, CancellationToken cancellationToken = default)
        {
            unitOfWork.PatientsRepository.Update(patient, cancellationToken);
        }

        public async Task<Patient?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.PatientsRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task SoftDeletePatient(Patient patient, CancellationToken cancellationToken = default)
        {
            unitOfWork.PatientsRepository.SoftDelete(patient, cancellationToken);
        }

        public async Task HardDeletePatient(Patient patient, CancellationToken cancellationToken = default)
        {
            unitOfWork.PatientsRepository.Delete(patient, cancellationToken);
        }

        public async Task<Patient?> GetPatientByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.PatientsRepository.GetPatientByPhoneAsync(phone, cancellationToken);
        }

        public async Task<List<Patient?>> GetPatientListByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.PatientsRepository.GetPatientsByNameAsync(name, cancellationToken)).ToList();
        }
    }
}
