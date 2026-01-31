namespace Clinic_System.Application.Features.Prescriptions.Commands.Handlers
{
    public class DeletePrescriptionCommandHandler : AppRequestHandler<DeletePrescriptionCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePrescriptionCommandHandler> _logger;

        // 2. تمرير CurrentUserService للـ Base
        public DeletePrescriptionCommandHandler(ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork, ILogger<CreatePrescriptionCommandHandler> logger) : base(currentUserService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task<Response<string>> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting prescription with ID: {PrescriptionId}", request.PrescriptionId);
            try
            {
                var prescription = await _unitOfWork.PrescriptionsRepository.GetPrescriptionWithDetailsAsync(request.PrescriptionId, cancellationToken);

                if (prescription == null) return BadRequest<string>("Not Found");

                var doctorId = prescription.MedicalRecord?.Appointment?.DoctorId;
                if (doctorId.HasValue)
                {
                    var authResult = await ValidateDoctorAccess(doctorId.Value);
                    if (authResult != null) return authResult;
                }
                else
                {
                    return BadRequest<string>("Critical Data Integrity Error.");
                }

                prescription.SoftDelete();

                _unitOfWork.PrescriptionsRepository.Update(prescription);

                var result = await _unitOfWork.SaveAsync();

                if (result == 0)
                {
                    _logger.LogWarning("Failed to delete prescription with ID: {PrescriptionId}", request.PrescriptionId);
                    return BadRequest<string>("Failed to delete prescription.");
                }
                return Success<string>(null, "Prescription deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting prescription with ID: {PrescriptionId}", request.PrescriptionId);
                return BadRequest<string>("An error occurred while deleting the prescription.");
            }
        }
    }
}
