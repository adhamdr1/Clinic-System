namespace Clinic_System.Application.Features.MedicalRecords.Queries.Models
{
    public class GetRecordsByDoctorIdQuery : IRequest<Response<PagedResult<MedicalRecordDoctorDTO>>>
    {
        public int DoctorId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
