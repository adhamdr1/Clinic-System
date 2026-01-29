namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class UpdateUserProfileCommandHandler : AppRequestHandler<UpdateUserProfileCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            ILogger<UpdateUserProfileCommandHandler> logger) : base(currentUserService)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public override async Task<Response<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            bool isAdmin = roles.Contains("Admin");
            string targetUserId = isAdmin && !string.IsNullOrEmpty(request.UserId) ? request.UserId : _currentUserService.UserId;

            _logger.LogInformation("Attempting to update profile for User: {TargetId}. Requested by: {CurrentUserId}", targetUserId, _currentUserService.UserId);

            try
            {
                var result = await _identityService.UpdateUserProfileAsync(
                    targetUserId,
                    request.NewEmail,
                    request.NewUserName,
                    request.CurrentPassword,
                    isAdmin
                );

                if (result)
                {
                    _logger.LogInformation("Successfully updated profile for User: {TargetId}", targetUserId);
                    return Success(true, "Profile updated successfully.");
                }

                _logger.LogWarning("Update failed for User: {TargetId} without throwing exception", targetUserId);
                return BadRequest<bool>("Update failed, please check your data.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized profile update attempt for User: {TargetId}", targetUserId);
                return Unauthorized<bool>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error during profile update for User: {TargetId}", targetUserId);
                return BadRequest<bool>($"An error occurred: {ex.Message}");
            }
        }
    }
}