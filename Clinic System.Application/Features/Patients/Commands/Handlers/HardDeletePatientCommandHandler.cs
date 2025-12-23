namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class HardDeletePatientCommandHandler : ResponseHandler, IRequestHandler<HardDeletePatientCommand, Response<string>>
    {
        private readonly IPatientService patientService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HardDeletePatientCommandHandler> logger;
        public HardDeletePatientCommandHandler(IPatientService patientService
            , IIdentityService identityService, IUnitOfWork unitOfWork, ILogger<HardDeletePatientCommandHandler> logger)
        {
            this.patientService = patientService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<string>> Handle(HardDeletePatientCommand request, CancellationToken cancellationToken)
        {

            var doctor = await patientService.GetPatientByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Patient with Id {PatientId} not found", request.Id);
                return NotFound<string>($"Patient with Id {request.Id} not found");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting hard delete for Patient with Id {PatientId}", request.Id);
                try
                {
                    await patientService.HardDeletePatient(doctor, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogError("Failed to hard delete Patient with Id {PatientId}", request.Id);
                        return BadRequest<string>("Failed to Deleted Patient");
                    }

                    var IsDeletedUser = await identityService.HardDeleteUserAsync(doctor.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        logger.LogError("Failed to hard delete associated user for Patient with Id {PatientId}", request.Id);
                        return BadRequest<string>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while hard deleting Patient with Id {PatientId}", request.Id);
                    return BadRequest<string>($"Patient deletion failed: {ex.Message}");
                }
            }

            logger.LogInformation("Patient with Id {PatientId} deleted successfully", request.Id);
            return Deleted<string>("Patient Deleted successfully");
        }
    }
}
