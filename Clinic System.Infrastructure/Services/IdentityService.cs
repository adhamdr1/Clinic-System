namespace Clinic_System.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<string> CreateUserAsync(string userName, string email, string password, string role, CancellationToken cancellationToken = default)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainException("User name cannot be empty");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email cannot be empty");

            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password cannot be empty");

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new DomainException($"Failed to create user: {errors}");
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new DomainException($"Failed to assign role: {roleErrors}");
                }
            }

            return user.Id;
        }

        public async Task<bool> SoftDeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            // Soft Delete: Set IsDeleted and DeletedAt
            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> HardDeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<string?> GetUserEmailAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }
        public async Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }
       
        public async Task<bool> ExistingEmail(string Email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == Email);
        }

        public async Task<bool> ExistingUserName(string UserName)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == UserName);
        }

        public async Task<bool> UpdateEmailUserAsync(string userId, string newEmail, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(newEmail))
                throw new DomainException("Email cannot be empty");


            var emailResult = await _userManager.SetEmailAsync(user , newEmail);

            if (!emailResult.Succeeded)
            {
                var errors = string.Join(", ", emailResult.Errors.Select(e => e.Description));
                throw new DomainException($"Failed to Update Email User: {errors}");
            }

            return emailResult.Succeeded;
        }

        public async Task<bool> UpdatePasswordUserAsync(string userId, string newpassword, string currentPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(newpassword))
                throw new DomainException("Password cannot be empty");

            var passwordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newpassword);

            if (!passwordResult.Succeeded)
            {
                var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                throw new DomainException($"Failed to Update Password User: {errors}");
            }

            return passwordResult.Succeeded;
        }

        public async Task<bool> UpdateUserNameAsync(string userId, string newUserName, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(newUserName))
                throw new DomainException("User name cannot be empty");

            var userNameResult = await _userManager.SetUserNameAsync(user, newUserName);

            if (!userNameResult.Succeeded)
            {
                var errors = string.Join(", ", userNameResult.Errors.Select(e => e.Description));
                throw new DomainException($"Failed to Update User Name User: {errors}");
            }

            return userNameResult.Succeeded;
        }
   
    }
}

