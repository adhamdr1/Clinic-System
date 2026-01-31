namespace Clinic_System.Application.Features.Prescriptions.Commands.Handlers
{
    public class UpdatePrescriptionCommandHandler : AppRequestHandler<UpdatePrescriptionCommand, PrescriptionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePrescriptionCommandHandler> _logger;
        public UpdatePrescriptionCommandHandler(ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<UpdatePrescriptionCommandHandler> logger) : base(currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public override async Task<Response<PrescriptionDto>> Handle(UpdatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating prescription with ID: {PrescriptionId}", request.PrescriptionId);
            try
            {
                var prescription = await _unitOfWork.PrescriptionsRepository.GetPrescriptionWithDetailsAsync(request.PrescriptionId, cancellationToken);

                if (prescription == null) return BadRequest<PrescriptionDto>("Prescription not found.");

                var doctorId = prescription.MedicalRecord?.Appointment?.DoctorId;

                if (doctorId.HasValue)
                {
                    var authResult = await ValidateDoctorAccess(doctorId.Value);
                    if (authResult != null) return authResult;
                }
                else
                {
                    return BadRequest<PrescriptionDto>("Critical Data Integrity Error.");
                }

                prescription.Update(request.MedicationName, request.Dosage,
                    request.SpecialInstructions, request.Frequency, request.StartDate, request.EndDate);

                _unitOfWork.PrescriptionsRepository.Update(prescription);

                var result = await _unitOfWork.SaveAsync(); 

                if (result == 0)
                {
                    _logger.LogWarning("Failed to update prescription with ID: {PrescriptionId}", request.PrescriptionId);
                    return BadRequest<PrescriptionDto>("Failed to update prescription.");
                }

                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);

                return Success<PrescriptionDto>(prescriptionDto, "Prescription updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating prescription with ID: {PrescriptionId}", request.PrescriptionId);
                return BadRequest<PrescriptionDto>("An error occurred while updating the prescription.");
            }
        }
    }
}
