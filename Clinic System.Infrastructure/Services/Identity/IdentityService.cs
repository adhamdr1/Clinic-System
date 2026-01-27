namespace Clinic_System.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            }

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

        public async Task<(bool IsAuthenticated, bool IsEmailConfirmed, string Id, string UserName, string Email, List<string> Roles)> LoginAsync(string userNameOrEmail, string password)
        {
            var user = userNameOrEmail.Contains("@")
                ? await _userManager.FindByEmailAsync(userNameOrEmail)
                : await _userManager.FindByNameAsync(userNameOrEmail);

            if (user == null)
            {
                return (false, false, string.Empty, string.Empty, string.Empty, new List<string>());
            }
            
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!signInResult.Succeeded)
            {
                return (false, false, string.Empty, string.Empty, string.Empty, new List<string>());
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return (true, false, user.Id, user.UserName!, user.Email!, new List<string>());
            }

            var roles = await _userManager.GetRolesAsync(user);

            return (true, true, user.Id, user.UserName!, user.Email!, roles.ToList());
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new DomainException("User not found");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public string EncodeToken(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        public string DecodeToken(string encodedToken)
        {
            var bytes = WebEncoders.Base64UrlDecode(encodedToken);
            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DomainException("User not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<(bool Succeeded, string Error)> ResetPasswordAsync(string email, string decodedToken, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "User not found");

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (result.Succeeded)
            {
                return (true, null); 
            }

            // ·Ê ›‘·° Ã„⁄ ﬂ· «·√Œÿ«¡ ›Ì —”«·… Ê«Õœ…
            // „À·«: "Password requires non-alphanumeric, Password requires digit"
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return (false, errors);
        }

        public async Task<(string UserId, string UserName, string Role, string Token, string Error)> GenerateTokenForResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return (null, null, null, null, "No user found with this email.");

            if (user.EmailConfirmed)
                return (null, null, null, null, "Email is already confirmed.");

            //  Ê·Ìœ  Êﬂ‰ ÃœÌœ
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var userRole = roles.FirstOrDefault();

            return (user.Id, user.UserName, userRole, token, null);
        }
    }
}