namespace Clinic_System.Application.Features.Doctors.Queries.Handlers
{
    public class MedicalRecordByIdQueryHandler : AppRequestHandler<GetMedicalRecordByIdQuery, MedicalRecordDTO>
    {
        private readonly IUnitOfWork unitOfWork; 
        private readonly IMapper mapper;
        private readonly ILogger<MedicalRecordByIdQueryHandler> logger;
        public MedicalRecordByIdQueryHandler(
            ICurrentUserService currentUserService, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MedicalRecordByIdQueryHandler> logger) : base(currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public override async Task<Response<MedicalRecordDTO>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var record = await unitOfWork.MedicalRecordsRepository.GetMedicalRecordDetailsAsync(request.Id, cancellationToken);

                if (record == null)
                {
                    return NotFound<MedicalRecordDTO>($"Medical record with ID {request.Id} not found.");
                }

                var doctorId = record.Appointment?.DoctorId;
                var patientId = record.Appointment?.PatientId;

                var roles = await _currentUserService.GetCurrentUserRolesAsync();
                if (!roles.Contains("Admin"))
                {
                    if (CurrentDoctorId.HasValue)
                    {
                        if (doctorId != CurrentDoctorId)
                            return Unauthorized<MedicalRecordDTO>("Access denied. You can only view your own records.");
                    }
                    else if (CurrentPatientId.HasValue)
                    {
                        if (patientId != CurrentPatientId)
                            return Unauthorized<MedicalRecordDTO>("Access denied. You can only view your own records.");
                    }
                    else
                    {
                        return Unauthorized<MedicalRecordDTO>("Access denied.");
                    }
                }


                var medicalRecordDto = mapper.Map<MedicalRecordDTO>(record);
                return Success(medicalRecordDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical record with ID {MedicalRecordId}", request.Id);
                return NotFound<MedicalRecordDTO>("An error occurred while processing your request.");
            }
        }
    }
}
