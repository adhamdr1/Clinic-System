namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListPagingQueryHandler : ResponseHandler , IRequestHandler<GetDoctorListPagingQuery, Response<PagedResult<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorListPagingQueryHandler(IDoctorService doctorService, IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<Response<PagedResult<GetDoctorListDTO>>> Handle(GetDoctorListPagingQuery request, CancellationToken cancellationToken)
        {
            var doctors = await doctorService.GetDoctorsListPagingAsync(request.PageNumber, request.PageSize, cancellationToken);

            if (doctors?.Items.Any() != true)
                return NotFound<PagedResult<GetDoctorListDTO>>();

            if (request.PageNumber < 1)
                return BadRequest<PagedResult<GetDoctorListDTO>>("Page number must be greater than 0");

            if (request.PageSize < 1 || request.PageSize > 100)
                return BadRequest<PagedResult<GetDoctorListDTO>>("Page size must be between 1 and 100");

            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors.Items);
            var pagedResult = new PagedResult<GetDoctorListDTO>(doctorsMapper, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);
            return Success(pagedResult);
        }
    }
}
