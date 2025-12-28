namespace Clinic_System.Application.Features.Patients.Queries.Handlers
{
    public class PatientListPagingQueryHandler : ResponseHandler, IRequestHandler<GetPatientListPagingQuery, Response<PagedResult<GetPatientListDTO>>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ILogger<PatientListPagingQueryHandler> logger;

        public PatientListPagingQueryHandler(IPatientService patientService, IMapper mapper, ILogger<PatientListPagingQueryHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<GetPatientListDTO>>> Handle(GetPatientListPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetPatientListPagingQuery: PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var patients = await patientService.GetPatientsListPagingAsync(request.PageNumber, request.PageSize, cancellationToken);

            if (patients?.Items.Any() != true)
            {
                logger.LogWarning("No doctors found for PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
                return NotFound<PagedResult<GetPatientListDTO>>();
            }

            var patientsMapper = mapper.Map<List<GetPatientListDTO>>(patients.Items);
            var pagedResult = new PagedResult<GetPatientListDTO>(patientsMapper, patients.TotalCount, patients.CurrentPage, patients.PageSize);

            logger.LogInformation("Successfully retrieved {Count} patients for PageNumber={PageNumber}, PageSize={PageSize}", patients.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
