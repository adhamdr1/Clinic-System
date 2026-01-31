namespace Clinic_System.Application.Features.Prescriptions.Commands.Handlers
{
    public class CreatePrescriptionCommandHandler : AppRequestHandler<CreatePrescriptionCommand, PrescriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePrescriptionCommandHandler> _logger;

        // 2. تمرير CurrentUserService للـ Base
        public CreatePrescriptionCommandHandler(ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreatePrescriptionCommandHandler> logger) : base(currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<Response<PrescriptionDto>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new prescription for MedicalRecordId: {MedicalRecordId}", request.MedicalRecordId);
            try
            {
                var record = await _unitOfWork.MedicalRecordsRepository.GetMedicalRecordWithAppointmentAsync(request.MedicalRecordId);

                if (record == null) return BadRequest<PrescriptionDto>("Medical record not found.");

                if (record.Appointment?.DoctorId == null)
                    return BadRequest<PrescriptionDto>("Invalid Record Data");

                var authResult = await ValidateDoctorAccess(record.Appointment.DoctorId);
                if (authResult != null) return authResult;

                var prescription = _mapper.Map<Prescription>(request);

                await _unitOfWork.PrescriptionsRepository.AddAsync(prescription);

                var result = await _unitOfWork.SaveAsync();

                if (result == 0)
                {
                    _logger.LogWarning("Failed to create prescription for MedicalRecordId: {MedicalRecordId}", request.MedicalRecordId);
                    return BadRequest<PrescriptionDto>("Failed to create prescription.");
                }

                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
                return Created<PrescriptionDto>(prescriptionDto, "Prescription created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating prescription.");
                return BadRequest<PrescriptionDto>("An error occurred while creating the prescription.");
            }
        }
    }
}
