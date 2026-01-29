namespace Clinic_System.Application.Features.Authentication.Commands.Handlers
{
    public class ChangeUserPasswordCommandHandler : AppRequestHandler<ChangeUserPasswordCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ChangeUserPasswordCommandHandler> _logger;

        public ChangeUserPasswordCommandHandler(
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            ILogger<ChangeUserPasswordCommandHandler> _logger) : base(currentUserService)
        {
            _identityService = identityService;
            this._logger = _logger;
        }

        public override async Task<Response<bool>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            bool isAdmin = roles.Contains("Admin");

            string targetUserId = isAdmin && !string.IsNullOrEmpty(request.UserId)
                                  ? request.UserId
                                  : _currentUserService.UserId;

            _logger.LogInformation("Password change initiated for User: {TargetId} by Actor: {ActorId}", targetUserId, _currentUserService.UserId);

            try
            {
                var result = await _identityService.ChangePasswordAsync(
                    targetUserId,
                    request.CurrentPassword,
                    request.NewPassword,
                    isAdmin,
                    cancellationToken
                );

                _logger.LogInformation("Password successfully changed for User: {TargetId}", targetUserId);
                return Success(true, "Password has been updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Failed password change attempt (Wrong Current Password) for User: {TargetId}", targetUserId);
                return Unauthorized<bool>("The current password you provided is incorrect.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error during password change for User: {TargetId}", targetUserId);
                return BadRequest<bool>($"An error occurred: {ex.Message}");
            }
        }
    }
}