namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorListByNameQuery : IRequest<Response<List<GetDoctorListDTO>>>
    {
        public string FullName { get; set; } = null!;
    }
}
