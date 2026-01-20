namespace Clinic_System.Application.Features.Authorization.Commands.Handlers
{
    public class PromoteDoctorToAdminCommandHandler : ResponseHandler,IRequestHandler<PromoteDoctorToAdminCommand, Response<string>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PromoteDoctorToAdminCommandHandler> _logger;
        
        public PromoteDoctorToAdminCommandHandler(IAuthorizationService authorizationService, IUnitOfWork unitOfWork, ILogger<PromoteDoctorToAdminCommandHandler> logger)
        {
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<string>> Handle(PromoteDoctorToAdminCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PromoteDoctorToAdminCommandHandler called with DoctorId: {DoctorId}", request.DoctorId);
            var doctor = await _unitOfWork.DoctorsRepository.GetByIdAsync(request.DoctorId);

            if (doctor == null)
                return NotFound<string>("Doctor not found"); 

            var userId = doctor.ApplicationUserId;

            // 3. نكلم الـ AuthorizationService عشان نضيف رول "Admin"
            var result = await _authorizationService.AddRoleAsync(userId, "Admin");

            _logger.LogInformation("AddRoleAsync result for UserId {UserId}: {Result}", userId, result);

            switch (result)
            {
                case "UserNotFound": return NotFound<string>("Identity User not found");
                case "RoleNotFound": return NotFound<string>("Role not found");
                case "UserAlreadyInRole": return BadRequest<string>("Doctor is already an Admin");
                case "Success": return Success($"Doctor For Id {doctor.Id} has been promoted to Admin successfully");
                default: return BadRequest<string>("Failed to add role");
            }
        }
    }
}
