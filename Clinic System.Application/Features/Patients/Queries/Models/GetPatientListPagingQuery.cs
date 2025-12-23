namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientListPagingQuery : IRequest<Response<PagedResult<GetPatientListDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
