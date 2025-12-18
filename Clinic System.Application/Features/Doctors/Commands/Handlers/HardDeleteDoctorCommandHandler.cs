namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class HardDeleteDoctorCommandHandler : ResponseHandler, IRequestHandler<HardDeleteDoctorCommand, Response<string>>
    {
        private readonly IDoctorService doctorService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HardDeleteDoctorCommandHandler> logger;
        public HardDeleteDoctorCommandHandler(IDoctorService doctorService
            , IIdentityService identityService, IUnitOfWork unitOfWork, ILogger<HardDeleteDoctorCommandHandler> logger)
        {
            this.doctorService = doctorService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<string>> Handle(HardDeleteDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with Id {DoctorId} not found", request.Id);
                return NotFound<string>($"Doctor with Id {request.Id} not found");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                logger.LogInformation("Starting hard delete for Doctor with Id {DoctorId}", request.Id);
                try
                {
                    await doctorService.HardDeleteDoctor(doctor, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogError("Failed to hard delete Doctor with Id {DoctorId}", request.Id);
                        return BadRequest<string>("Failed to Deleted doctor");
                    }

                    var IsDeletedUser = await identityService.HardDeleteUserAsync(doctor.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        logger.LogError("Failed to hard delete associated user for Doctor with Id {DoctorId}", request.Id);
                        return BadRequest<string>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while hard deleting Doctor with Id {DoctorId}", request.Id);
                    return BadRequest<string>($"Doctor deletion failed: {ex.Message}");
                }
            }

            logger.LogInformation("Doctor with Id {DoctorId} deleted successfully", request.Id);
            return Deleted<string>("Doctor Deleted successfully");
        }
    }
}
