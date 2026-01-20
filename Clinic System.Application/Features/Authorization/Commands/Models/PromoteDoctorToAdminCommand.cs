namespace Clinic_System.Application.Features.Authorization.Commands.Models
{
    public class PromoteDoctorToAdminCommand : IRequest<Response<string>>
    {
        public int DoctorId { get; set; }
    }
}
