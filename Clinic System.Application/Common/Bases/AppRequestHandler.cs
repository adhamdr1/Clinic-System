namespace Clinic_System.Application.Common.Bases
{
    public abstract class AppRequestHandler<TRequest, TResponse> : ResponseHandler, IRequestHandler<TRequest, Response<TResponse>>
        where TRequest : IRequest<Response<TResponse>>
    {
        protected readonly ICurrentUserService _currentUserService;

        public AppRequestHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        protected string CurrentUserId => _currentUserService.UserId;

        protected int? CurrentDoctorId => _currentUserService.DoctorId;
        protected int? CurrentPatientId => _currentUserService.PatientId;

        protected async Task<Response<TResponse>> ValidateOwner(string entityUserId)
        {

            var roles = await _currentUserService.GetCurrentUserRolesAsync();

            if (roles.Contains("Admin"))
            {
                return null;
            }

            if (entityUserId != CurrentUserId)
            {
                return Unauthorized<TResponse>("You do not have permission to access this resource.");
            }
            return null;
        }

        protected async Task<Response<TResponse>> ValidateDoctorAccess(int targetDoctorId)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            if (roles.Contains("Admin")) return null;

            // لو أنا مش دكتور أصلاً، أو لو أنا دكتور بس مش هو ده رقمي
            if (CurrentDoctorId != targetDoctorId)
            {
                return Unauthorized<TResponse>("Access denied. You can only view your own data.");
            }
            return null;
        }

        protected async Task<Response<TResponse>> ValidatePatientAccess(int targetPatientId)
        {
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            if (roles.Contains("Admin")) return null;

            // لو أنا مش دكتور أصلاً، أو لو أنا دكتور بس مش هو ده رقمي
            if (CurrentPatientId != targetPatientId)
            {
                return Unauthorized<TResponse>("Access denied. You can only view your own data.");
            }
            return null;
        }

        public abstract Task<Response<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}