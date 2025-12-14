namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorListBySpecializationQuery : IRequest<Response<List<GetDoctorListDTO>>>
    {
        public string Specialization { get; set; } = null!;
    }
}
