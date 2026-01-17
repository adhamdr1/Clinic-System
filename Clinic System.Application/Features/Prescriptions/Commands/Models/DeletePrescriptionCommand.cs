namespace Clinic_System.Application.Features.Prescriptions.Commands.Models
{
    public class DeletePrescriptionCommand : IRequest<Response<string>>
    {
        public int PrescriptionId { get; set; }
    }
}