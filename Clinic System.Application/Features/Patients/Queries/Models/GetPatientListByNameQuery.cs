namespace Clinic_System.Application.Features.Patients.Queries.Models
{
    public class GetPatientListByNameQuery : IRequest<Response<List<GetPatientListDTO>>>
    {
        public string FullName { get; set; } = null!;
    }
}
