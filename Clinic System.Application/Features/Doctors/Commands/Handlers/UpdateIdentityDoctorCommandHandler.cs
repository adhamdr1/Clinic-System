namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class UpdateIdentityDoctorCommandHandler : ResponseHandler, IRequestHandler<UpdateIdentityDoctorCommand, Response<UserDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdateIdentityDoctorCommandHandler> logger;

        public UpdateIdentityDoctorCommandHandler(IDoctorService doctorService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork, ILogger<UpdateIdentityDoctorCommandHandler> logger)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<UserDTO>> Handle(UpdateIdentityDoctorCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateIdentityDoctorCommand for Doctor Id: {DoctorId}", request.Id);

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                logger.LogWarning("Doctor with Id: {DoctorId} not found", request.Id);
                return NotFound<UserDTO>($"Doctor with Id {request.Id} not found");
            }

            var oldEmail = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken);
            var oldUserName = await identityService.GetUserNameAsync(doctor.ApplicationUserId, cancellationToken);


            bool isEmailChanged = !string.IsNullOrEmpty(request.Email) && request.Email != oldEmail;
            bool isUserNameChanged = !string.IsNullOrEmpty(request.UserName) && request.UserName != oldUserName;
            bool isPasswordChanged = !string.IsNullOrEmpty(request.Password);

            if (isEmailChanged && string.IsNullOrEmpty(request.UserName))
            { 
                logger.LogWarning("Email change requested without Username for Doctor Id: {DoctorId}", request.Id);
                return BadRequest<UserDTO>("To change Email, you must provide Username.");
            }

            if (isUserNameChanged && string.IsNullOrEmpty(request.Email))
            {
                logger.LogWarning("Username change requested without Email for Doctor Id: {DoctorId}", request.Id);
                return BadRequest<UserDTO>("To change Username, you must provide Email.");
            }

            if (isPasswordChanged && string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.UserName))
            {
                logger.LogWarning("Password change requested without Email or Username for Doctor Id: {DoctorId}", request.Id);
                return BadRequest<UserDTO>("To change Password, you must provide Email or Username.");
            }

            bool passwordUpdated = false;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    logger.LogInformation("Starting transaction for updating identity of Doctor Id: {DoctorId}", request.Id);
                    if (isEmailChanged)
                    {
                        logger.LogInformation("Updating email for Doctor Id: {DoctorId}", request.Id);
                        await identityService.UpdateEmailUserAsync(doctor.ApplicationUserId, request.Email, cancellationToken);
                    }

                    if (isUserNameChanged)
                    {
                        logger.LogInformation("Updating username for Doctor Id: {DoctorId}", request.Id);
                        await identityService.UpdateUserNameAsync(doctor.ApplicationUserId, request.UserName, cancellationToken);
                    }

                    if (isPasswordChanged)
                    {
                        logger.LogInformation("Updating password for Doctor Id: {DoctorId}", request.Id);
                        await identityService.UpdatePasswordUserAsync(
                            doctor.ApplicationUserId,
                            request.Password,
                            request.CurrentPassword,
                            cancellationToken
                        );
                        passwordUpdated = true;
                    }

                    // تحديث بيانات Doctor (غير الهوية)
                    mapper.Map(request, doctor);

                    await unitOfWork.SaveAsync();

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating identity for Doctor Id: {DoctorId}", request.Id);
                    return BadRequest<UserDTO>($"Update failed: Because Current Password Invalid");
                }

                var userDTO = new UserDTO
                {
                    Id = doctor.Id,
                    UserId = doctor.ApplicationUserId,
                    FullName = doctor.FullName,
                    UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : oldUserName,
                    Email = !string.IsNullOrEmpty(request.Email) ? request.Email : oldEmail
                };

                string message = "Doctor identity updated successfully.";

                if (passwordUpdated)
                    message += " Password updated.";

                logger.LogInformation("Successfully updated identity for Doctor Id: {DoctorId}", request.Id);
                return Success<UserDTO>(userDTO, message);
            }
        }
    }
}