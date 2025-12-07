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

        //public async Task<PagedResult<Doctor>> GetDoctorsListPagingAsync(int pageNumber, int pageSize)
        //{
        //    var (items,totalCount) = await unitOfWork.DoctorsRepository.GetPaginatedAsync(pageNumber, pageSize);

        //    return new PagedResult<Doctor>(items, totalCount, pageNumber, pageSize);
        //}
    }
}
