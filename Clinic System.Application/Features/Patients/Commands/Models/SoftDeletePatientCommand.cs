namespace Clinic_System.Application.Features.Patients.Commands.Models
{
    public class SoftDeletePatientCommand : IRequest<Response<Patient>>
    {
        public int Id { get; set; }
    }
}
