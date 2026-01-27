namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorListByNameQuery : IRequest<Response<List<GetDoctorBasicInfoDTO>>>
    {
        public string FullName { get; set; } = null!;
    }
}
