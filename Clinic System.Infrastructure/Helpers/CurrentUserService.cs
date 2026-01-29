namespace Clinic_System.Infrastructure.Helpers
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<List<string>> GetCurrentUserRolesAsync()
        {
            var roles = _httpContextAccessor.HttpContext?.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Task.FromResult(roles ?? new List<string>());
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int? DoctorId
        {
            get
            {
                // بنبحث عن Claim اسمه "DoctorId" اللي حطيناه واحنا بنعمل التوكن
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst("DoctorId")?.Value;

                // لو موجود وقيمته رقم صحيح، بنرجعه.. غير كده بنرجع null
                if (int.TryParse(claimValue, out int id))
                {
                    return id;
                }
                return null;
            }
        }

        public int? PatientId
        {
            get
            {
                // نفس الكلام للمريض
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst("PatientId")?.Value;

                if (int.TryParse(claimValue, out int id))
                {
                    return id;
                }
                return null;
            }
        }

    }
}