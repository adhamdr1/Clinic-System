namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class DoctorListBySpecializationQueryHandler : ResponseHandler, IRequestHandler<GetDoctorListBySpecializationQuery, Response<List<GetDoctorListDTO>>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;

        public DoctorListBySpecializationQueryHandler(IDoctorService doctorService, IMapper mapper)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
        }

        public async Task<Response<List<GetDoctorListDTO>>> Handle(GetDoctorListBySpecializationQuery request, CancellationToken cancellationToken)
        {
            var doctors = await doctorService.GetDoctorsListBySpecializationAsync(request.Specialization, cancellationToken);

            if (doctors?.Any() != true)
                return NotFound<List<GetDoctorListDTO>>();


            var doctorsMapper = mapper.Map<List<GetDoctorListDTO>>(doctors);
            return Success(doctorsMapper);
        }
    }
}
