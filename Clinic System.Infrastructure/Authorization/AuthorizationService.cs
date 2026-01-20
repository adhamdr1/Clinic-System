namespace Clinic_System.Infrastructure.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> AddRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "UserNotFound";

            // نتأكد إن الرول موجودة أصلاً في السيستم
            if (!await IsRoleExistAsync(roleName))
                return "RoleNotFound";

            // نتأكد إن اليوزر مش واخد الرول دي قبل كده
            if (await _userManager.IsInRoleAsync(user, roleName))
                return "UserAlreadyInRole";

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded) return "Success";

            return "Failed";
        }

        public async Task<bool> IsRoleExistAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
