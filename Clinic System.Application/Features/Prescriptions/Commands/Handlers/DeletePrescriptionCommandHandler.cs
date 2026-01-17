namespace Clinic_System.Application.Features.Prescriptions.Commands.Handlers
{
    public class DeletePrescriptionCommandHandler : ResponseHandler, IRequestHandler<DeletePrescriptionCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePrescriptionCommandHandler> _logger;
        public DeletePrescriptionCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePrescriptionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Response<string>> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting prescription with ID: {PrescriptionId}", request.PrescriptionId);
            try
            {
                var prescription = await _unitOfWork.PrescriptionsRepository.GetByIdAsync(request.PrescriptionId);
                if (prescription == null)
                {
                    _logger.LogWarning("Prescription with ID {PrescriptionId} not found.", request.PrescriptionId);
                    return BadRequest<string>("Prescription not found.");
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
