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

        public abstract Task<Response<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
