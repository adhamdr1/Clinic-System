namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class SoftDeleteDoctorCommandHandler : ResponseHandler, IRequestHandler<SoftDeleteDoctorCommand, Response<Doctor>>
    {
        private readonly IDoctorService doctorService;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SoftDeleteDoctorCommandHandler> logger;

        public SoftDeleteDoctorCommandHandler(IDoctorService doctorService
            , IIdentityService identityService, IUnitOfWork unitOfWork, ILogger<SoftDeleteDoctorCommandHandler> logger)
        {
            this.doctorService = doctorService;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<Doctor>> Handle(SoftDeleteDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with Id {DoctorId} not found", request.Id);
                return NotFound<Doctor>($"Doctor with Id {request.Id} not found");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    logger.LogInformation("Soft deleting Doctor with Id {DoctorId}", request.Id);
                    await doctorService.SoftDeleteDoctor(doctor, cancellationToken);

                    var result = await unitOfWork.SaveAsync();
                    if (result == 0)
                    {
                        logger.LogError("Failed to delete Doctor with Id {DoctorId}", request.Id);
                        return BadRequest<Doctor>("Failed to Deleted doctor");
                    }

                    var IsDeletedUser = await identityService.SoftDeleteUserAsync(doctor.ApplicationUserId, cancellationToken);

                    if (!IsDeletedUser)
                    {
                        logger.LogError("Failed to delete associated user for Doctor with Id {DoctorId}", request.Id);
                        return BadRequest<Doctor>("Failed to Deleted associated user");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while deleting Doctor with Id {DoctorId}", request.Id);
                    return BadRequest<Doctor>($"Doctor deletion failed: {ex.Message}");
                }
            }

            logger.LogInformation("Doctor with Id {DoctorId} deleted successfully", request.Id);
            return Deleted<Doctor>("Doctor Deleted successfully");
        }
    }
}
