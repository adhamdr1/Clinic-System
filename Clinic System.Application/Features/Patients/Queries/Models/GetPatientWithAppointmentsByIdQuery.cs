namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientWithAppointmentsByIdQuery : IRequest<Response<GetPatientWhitAppointmentDTO>>
    {
        public int Id { get; set; }
    }
}
