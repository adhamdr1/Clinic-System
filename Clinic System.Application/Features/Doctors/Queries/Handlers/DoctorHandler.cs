namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    internal class DoctorHandler : IRequestHandler<GetDoctorListQuery, List<GetDoctorList>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorHandler(IDoctorService doctorService,IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<List<GetDoctorList>> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            var doctors =await doctorService.GetDoctorsListAsync();
            var doctorsMapper = mapper.Map<List<GetDoctorList>>(doctors);
            return doctorsMapper;
        }
    }
}
