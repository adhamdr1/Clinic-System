namespace Clinic_System.Application.Features.Doctors.Commands.Handlers
{
    public class UpdateIdentityDoctorCommandHandler : ResponseHandler, IRequestHandler<UpdateIdentityDoctorCommand, Response<UserDTO>>
    {
        private readonly IDoctorService doctorService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUnitOfWork unitOfWork;

        public UpdateIdentityDoctorCommandHandler(IDoctorService doctorService
            , IMapper mapper, IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            this.doctorService = doctorService;
            this.mapper = mapper;
            this.identityService = identityService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<UserDTO>> Handle(UpdateIdentityDoctorCommand request, CancellationToken cancellationToken)
        {

            var doctor = await doctorService.GetDoctorByIdAsync(request.Id);

            if (doctor == null)
            {
                return NotFound<UserDTO>($"Doctor with Id {request.Id} not found");
            }

            var oldEmail = await identityService.GetUserEmailAsync(doctor.ApplicationUserId, cancellationToken);
            var oldUserName = await identityService.GetUserNameAsync(doctor.ApplicationUserId, cancellationToken);


            bool isEmailChanged = !string.IsNullOrEmpty(request.Email) && request.Email != oldEmail;
            bool isUserNameChanged = !string.IsNullOrEmpty(request.UserName) && request.UserName != oldUserName;
            bool isPasswordChanged = !string.IsNullOrEmpty(request.Password);

            if (isEmailChanged && string.IsNullOrEmpty(request.UserName))
                return BadRequest<UserDTO>("To change Email, you must provide Username.");

            if (isUserNameChanged && string.IsNullOrEmpty(request.Email))
                return BadRequest<UserDTO>("To change Username, you must provide Email.");

            if (isPasswordChanged && string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.UserName))
                return BadRequest<UserDTO>("To change Password, you must provide Email or Username.");



            bool passwordUpdated = false;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (isEmailChanged)
                        await identityService.UpdateEmailUserAsync(doctor.ApplicationUserId, request.Email, cancellationToken);

                    if (isUserNameChanged)
                        await identityService.UpdateUserNameAsync(doctor.ApplicationUserId, request.UserName, cancellationToken);

                    if (isPasswordChanged)
                    {
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

                return Success<UserDTO>(userDTO, message);
            }
        }
    }
}