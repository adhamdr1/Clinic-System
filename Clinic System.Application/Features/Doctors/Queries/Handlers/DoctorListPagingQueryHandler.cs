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

            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors.Items);
            var pagedResult = new PagedResult<GetDoctorListDTO>(doctorsMapper, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);
           
            logger.LogInformation("Successfully retrieved {Count} doctors for PageNumber={PageNumber}, PageSize={PageSize}", doctors.Items.Count(), request.PageNumber, request.PageSize);

            return Success(pagedResult);
        }
    }
}
