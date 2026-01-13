namespace Clinic_System.Application.Features.MedicalRecords.Queries.Models
{
    public class GetMedicalRecordByIdQuery : IRequest<Response<MedicalRecordDTO>>
    {
        public int Id { get; set; }
    }
}
