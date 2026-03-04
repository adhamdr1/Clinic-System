namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class UpdateDoctorCommandHandler : AppRequestHandler<UpdateDoctorCommand, UpdateDoctorDTO>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;
        private readonly ILogger<UpdateDoctorCommandHandler> logger;

        public UpdateDoctorCommandHandler(IDoctorService doctorService, ICurrentUserService currentUserService
            , IMapper mapper, IUnitOfWork unitOfWork, ICacheService cacheService, ILogger<UpdateDoctorCommandHandler> logger) : base(currentUserService) //
        {  
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public override async Task<Response<UpdateDoctorDTO>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting update process for doctor profile with Id {DoctorId}.", request.Id);

            var authResult = await ValidateDoctorAccess(request.Id);
            if (authResult != null)
                return authResult;

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with Id {DoctorId} not found.", request.Id);
                return NotFound<UpdateDoctorDTO>($"Doctor with Id {request.Id} not found");
            }

            // 1. امسك التخصص القديم قبل التعديل
            var oldSpecialization = doctor.Specialization.Trim().ToLower();

            mapper.Map(request, doctor);

            await doctorService.UpdateDoctor(doctor, cancellationToken);

            var result = await unitOfWork.SaveAsync();

            if (result == 0)
            {
                logger.LogError("Failed to update doctor profile with Id {DoctorId} in the database.", request.Id);
                return BadRequest<UpdateDoctorDTO>("Failed to update doctor profile in the database.");
            }

            var doctorsMapper = mapper.Map<UpdateDoctorDTO>(doctor);
            var newSpecialization = doctorsMapper.Specialization.Trim().ToLower();


            logger.LogInformation("Doctor profile with Id {DoctorId} updated successfully.", request.Id);

            await cacheService.RemoveByPrefixAsync(
                "DoctorsList",                                  // 1. بيمسح كل صفحات ليستة الدكاترة
                $"DoctorListBySpecialization:{oldSpecialization}", // 2. بيمسح كل صفحات التخصصات القديمة
                $"DoctorListBySpecialization:{newSpecialization}", // 2. بيمسح كل صفحات التخصصات الجديدة
                $"DoctorProfile_{request.Id}",                  // 3. بيمسح البروفايل القديم بتاع الدكتور ده
                $"DoctorWithAppointmentsById:{request.Id}"      // 4. بيمسح مواعيد الدكتور ده
            );

            return Success<UpdateDoctorDTO>(doctorsMapper, "Doctor updated successfully");
        }
    }
}