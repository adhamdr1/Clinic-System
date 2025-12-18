namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListPagingQueryHandler : ResponseHandler , IRequestHandler<GetDoctorListPagingQuery, Response<PagedResult<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly ILogger<DoctorListPagingQueryHandler> logger;

        public DoctorListPagingQueryHandler(IDoctorService doctorService, IMapper mapper, ILogger<DoctorListPagingQueryHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Response<PagedResult<GetDoctorListDTO>>> Handle(GetDoctorListPagingQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetDoctorListPagingQuery: PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var doctors = await doctorService.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, cancellationToken);

            if (doctors?.Items.Any() != true)
            {
                logger.LogWarning("No doctors found for PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
                return NotFound<PagedResult<GetDoctorListDTO>>();
            }

            if (request.PageNumber < 1)
            {
                logger.LogWarning("Invalid PageNumber={PageNumber} requested", request.PageNumber);
                return BadRequest<PagedResult<GetDoctorListDTO>>("Page number must be greater than 0");
            }

            if (request.PageSize < 1 || request.PageSize > 100)
            {
                logger.LogWarning("Invalid PageSize={PageSize} requested", request.PageSize);
                return BadRequest<PagedResult<GetDoctorListDTO>>("Page size must be between 1 and 100");
            }

            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors.Items);
            var pagedResult = new PagedResult<GetDoctorListDTO>(doctorsMapper, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);
           
            logger.LogInformation("Successfully retrieved {Count} doctors for PageNumber={PageNumber}, PageSize={PageSize}", doctors.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
