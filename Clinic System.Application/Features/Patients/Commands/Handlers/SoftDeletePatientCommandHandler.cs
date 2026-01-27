namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class SoftDeletePatientCommandHandler : AppRequestHandler<SoftDeletePatientCommand, Patient>
    {
        private readonly IPatientService patientService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SoftDeletePatientCommandHandler> logger;

        public SoftDeletePatientCommandHandler(
            ICurrentUserService currentUserService, 
            IPatientService patientService,
            IIdentityService identityService,
            IUnitOfWork unitOfWork,
            ILogger<SoftDeletePatientCommandHandler> logger) : base(currentUserService) // <--- تمرير للأب
        {
            this.patientService = patientService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public override async Task<Response<Patient>> Handle(SoftDeletePatientCommand request, CancellationToken cancellationToken)
        {

            var patient = await patientService.GetPatientByIdAsync(request.Id);

            if (patient == null)
            {
                logger.LogWarning("Patient with Id {PatientId} not found", request.Id);
                return NotFound<Patient>($"Patient with Id {request.Id} not found");
            }

            var authResult = await ValidateOwner(patient.ApplicationUserId);
            if (authResult != null) return authResult;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    logger.LogInformation("Soft deleting Patient with Id {PatientId}", request.Id);
                    await patientService.SoftDeletePatient(patient, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogError("Failed to delete Patient with Id {PatientId}", request.Id);
                        return BadRequest<Patient>("Failed to Deleted Patient");
                    }

                    var IsDeletedUser = await identityService.SoftDeleteUserAsync(patient.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        logger.LogError("Failed to delete associated user for Patient with Id {PatientId}", request.Id);
                        return BadRequest<Patient>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while deleting Patient with Id {PatientId}", request.Id);
                    return BadRequest<Patient>($"Patient deletion failed: {ex.Message}");
                }
            }

            logger.LogInformation("Patient with Id {PatientId} deleted successfully", request.Id);
            return Deleted<Patient>("Patient Deleted successfully");
        }
    }
}
