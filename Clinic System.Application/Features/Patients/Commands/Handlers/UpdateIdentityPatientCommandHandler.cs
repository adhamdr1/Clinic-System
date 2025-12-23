namespace Clinic_System.Application.Features.Patients.Commands.Handlers
{
    public class UpdateIdentityPatientCommandHandler : ResponseHandler, IRequestHandler<UpdateIdentityPatientCommand, Response<UserDTO>>
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdateIdentityPatientCommandHandler> logger;

        public UpdateIdentityPatientCommandHandler(IPatientService patientService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork, ILogger<UpdateIdentityPatientCommandHandler> logger)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Response<UserDTO>> Handle(UpdateIdentityPatientCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateIdentityPatientCommand for Patient Id: {PatientId}", request.Id);

            var patient = await patientService.GetPatientByIdAsync(request.Id);

            if (patient == null)
            {
                logger.LogWarning("Patient with Id: {PatientId} not found", request.Id);
                return NotFound<UserDTO>($"Patient with Id {request.Id} not found");
            }

            var oldEmail = await identityService.GetUserEmailAsync(patient.ApplicationUserId, cancellationToken);
            var oldUserName = await identityService.GetUserNameAsync(patient.ApplicationUserId, cancellationToken);


            bool isEmailChanged = !string.IsNullOrEmpty(request.Email) && request.Email != oldEmail;
            bool isUserNameChanged = !string.IsNullOrEmpty(request.UserName) && request.UserName != oldUserName;
            bool isPasswordChanged = !string.IsNullOrEmpty(request.Password);

            if (isEmailChanged && string.IsNullOrEmpty(request.UserName))
            {
                logger.LogWarning("Email change requested without Username for Patient Id: {PatientId}", request.Id);
                return BadRequest<UserDTO>("To change Email, you must provide Username.");
            }

            if (isUserNameChanged && string.IsNullOrEmpty(request.Email))
            {
                logger.LogWarning("Username change requested without Email for Patient Id: {PatientId}", request.Id);
                return BadRequest<UserDTO>("To change Username, you must provide Email.");
            }

            if (isPasswordChanged && string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.UserName))
            {
                logger.LogWarning("Password change requested without Email or Username for Patient Id: {PatientId}", request.Id);
                return BadRequest<UserDTO>("To change Password, you must provide Email or Username.");
            }

            bool passwordUpdated = false;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    logger.LogInformation("Starting transaction for updating identity of Patient Id: {PatientId}", request.Id);
                    if (isEmailChanged)
                    {
                        logger.LogInformation("Updating email for Patient Id: {PatientId}", request.Id);
                        await identityService.UpdateEmailUserAsync(patient.ApplicationUserId, request.Email, cancellationToken);
                    }

                    if (isUserNameChanged)
                    {
                        logger.LogInformation("Updating username for Patient Id: {PatientId}", request.Id);
                        await identityService.UpdateUserNameAsync(patient.ApplicationUserId, request.UserName, cancellationToken);
                    }

                    if (isPasswordChanged)
                    {
                        logger.LogInformation("Updating password for Patient Id: {PatientId}", request.Id);
                        await identityService.UpdatePasswordUserAsync(
                            patient.ApplicationUserId,
                            request.Password,
                            request.CurrentPassword,
                            cancellationToken
                        );
                        passwordUpdated = true;
                    }

                    // تحديث بيانات Doctor (غير الهوية)
                    mapper.Map(request, patient);

                    await unitOfWork.SaveAsync();

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating identity for Patient Id: {PatientId}", request.Id);
                    return BadRequest<UserDTO>($"Update failed: Because Current Password Invalid");
                }

                var userDTO = new UserDTO
                {
                    Id = patient.Id,
                    UserId = patient.ApplicationUserId,
                    FullName = patient.FullName,
                    UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : oldUserName,
                    Email = !string.IsNullOrEmpty(request.Email) ? request.Email : oldEmail
                };

                string message = "Patient identity updated successfully.";

                if (passwordUpdated)
                    message += " Password updated.";

                logger.LogInformation("Successfully updated identity for Patient Id: {PatientId}", request.Id);
                return Success<UserDTO>(userDTO, message);
            }
        }
    }
}