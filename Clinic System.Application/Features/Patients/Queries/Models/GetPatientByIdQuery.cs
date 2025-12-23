namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientByIdQuery : IRequest<Response<GetPatientDTO>>
    {
        public int Id { get; set; }
    }
}
