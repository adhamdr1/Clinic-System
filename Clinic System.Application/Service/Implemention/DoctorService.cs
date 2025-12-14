namespace Clinic_System.Application.Service.Implemention
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<Doctor>> GetDoctorsListAsync(CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.DoctorsRepository
                .GetAllAsync(cancellationToken: cancellationToken)).ToList();
        }

        public async Task<PagedResult<Doctor>> GetDoctorsListPagingAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await unitOfWork.DoctorsRepository.GetPaginatedAsync(pageNumber, pageSize, cancellationToken: cancellationToken);

            return new PagedResult<Doctor>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<Doctor?> GetDoctorWithAppointmentsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.DoctorsRepository.GetDoctorWithAppointmentsByIdAsync(id, cancellationToken);
        }
        public async Task CreateDoctorAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            await unitOfWork.DoctorsRepository.AddAsync(doctor, cancellationToken);
        }

        public async Task UpdateDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        {
            unitOfWork.DoctorsRepository.Update(doctor, cancellationToken);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.DoctorsRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task SoftDeleteDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        {
            unitOfWork.DoctorsRepository.SoftDelete(doctor, cancellationToken);
        }

        public async Task HardDeleteDoctor(Doctor doctor, CancellationToken cancellationToken = default)
        {
            unitOfWork.DoctorsRepository.Delete(doctor, cancellationToken);
        }

        public async Task<List<Doctor>> GetDoctorsListBySpecializationAsync(string Specialization, CancellationToken cancellationToken = default)
        {
            return (await unitOfWork.DoctorsRepository
                .GetDoctorsBySpecializationAsync(Specialization, cancellationToken)).ToList();
        }
    }
}
