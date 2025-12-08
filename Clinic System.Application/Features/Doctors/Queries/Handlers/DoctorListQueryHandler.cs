namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListQueryHandler : ResponseHandler, IRequestHandler<GetDoctorListQuery, Response<List<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorListQueryHandler(IDoctorService doctorService,IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetDoctorListDTO>>> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            var doctors = await doctorService.GetDoctorsListAsync(cancellationToken);
            if (doctors?.Any() != true)
                return NotFound<List<GetDoctorListDTO>>();



            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors);
            return Success(doctorsMapper);
        }
    }
}
