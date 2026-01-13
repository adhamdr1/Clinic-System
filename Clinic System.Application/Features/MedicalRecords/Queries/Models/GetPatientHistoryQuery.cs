namespace Clinic_System.Application.Features.MedicalRecords.Queries.Models
{
    public class GetPatientHistoryQuery : IRequest<Response<PagedResult<MedicalRecordPatientHistoryDTO>>>
    {
        public int PatientId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
