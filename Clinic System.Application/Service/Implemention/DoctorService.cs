namespace Clinic_System.Application.Service.Implemention
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<Doctor>> GetDoctorsListAsync()
        {
            return (List<Doctor>)await unitOfWork.DoctorsRepository.GetAllAsync();
        }
    }
}
