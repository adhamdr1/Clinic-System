namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorHandler :ResponseHandler, IRequestHandler<GetDoctorListQuery, Response<List<GetDoctorList>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorHandler(IDoctorService doctorService,IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetDoctorList>>> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            var doctors = await doctorService.GetDoctorsListAsync();
            if (doctors == null)
                return NotFound<List<GetDoctorList>>();



            var doctorsMapper = mapper.Map<List<GetDoctorList>>(doctors);
            return Success(doctorsMapper);
        }
    }
}
