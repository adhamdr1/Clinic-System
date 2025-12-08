namespace Clinic_System.Application.Features.Doctors.Queries.Models
{
    public class GetDoctorWithAppointmentsByIdQuery : IRequest<Response<GetDoctorDTO>>
    {
        public int Id { get; set; }
    }
}
