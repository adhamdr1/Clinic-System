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

        protected async Task<(int TargetId, Response<TResponse>? Error)> GetAuthorizedDoctorId(int? requestDoctorId)
        {
            // 1. لو دكتور: تجاهل الريكويست وخد الـ ID من التوكن
            if (CurrentDoctorId.HasValue)
            {
                return (CurrentDoctorId.Value, null);
            }

            // 2. لو مش دكتور: اتأكد إنه أدمن
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            if (roles.Contains("Admin"))
            {
                if (requestDoctorId == null || requestDoctorId == 0)
                {
                    return (0, BadRequest<TResponse>("DoctorId is required for Admin users."));
                }

                return (requestDoctorId.Value, null); // error
            }

            // 3. لو ولا ده ولا ده: رجع Error
            return (0, Unauthorized<TResponse>("Access denied. Only Doctors or Admins can view this data."));
        }

        protected async Task<(int TargetId, Response<TResponse>? Error)> GetAuthorizedPatientId(int? requestPatientId)
        {
            // 1. لو دكتور: تجاهل الريكويست وخد الـ ID من التوكن
            if (CurrentPatientId.HasValue)
            {
                return (CurrentPatientId.Value, null);
            }

            // 2. لو مش دكتور: اتأكد إنه أدمن
            var roles = await _currentUserService.GetCurrentUserRolesAsync();
            if (roles.Contains("Admin"))
            {
                if (requestPatientId == null || requestPatientId == 0)
                {
                    return (0, BadRequest<TResponse>("PatientId is required for Admin users."));
                }

                return (requestPatientId.Value, null);
            }

            // 3. لو ولا ده ولا ده: رجع Error
            return (0, Unauthorized<TResponse>("Access denied. Only Patients or Admins can view this data."));
        }

        public abstract Task<Response<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}