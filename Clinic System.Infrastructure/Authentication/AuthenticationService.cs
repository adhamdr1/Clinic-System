namespace Clinic_System.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt, string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(
             string userId, string userName, string email, List<string> roles, List<Claim>? extraClaims = null)
        {
            // 1. تحويل المفتاح السري (الختم) من نص لمفتاح تشفير
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecritKey));

            // 2. تجهيز البيانات (Claims) اللي هتتحفر جوه التوكن
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId), 
                new Claim(JwtRegisteredClaimNames.UniqueName, userName), 
                new Claim(JwtRegisteredClaimNames.Email, email), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (extraClaims != null)
            {
                authClaims.AddRange(extraClaims);
            }

            // 3. تحديد وقت انتهاء الكارنيه (التوكن)
            var expiresAt = DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationInMinutes);

            // 4. إنشاء كائن التوكن بكل تفاصيله
            var tokenObject = new JwtSecurityToken(
                issuer: _jwtSettings.IssuerIP,
                audience: _jwtSettings.AudienceIP,
                expires: expiresAt,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 5. تحويل الكائن لسطر نصي طويل (String)
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);


            var refreshToken = GenerateRefreshToken();

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.RefreshTokens ??= new List<RefreshToken>(); 
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            
            // 6. إرجاع الـ Tuple المطلوبة للـ Handler
            return (accessToken, refreshToken.Token , expiresAt, userName, email, roles);
        }

        public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            // 1. استخراج بيانات المستخدم من الـ Access Token المنتهية (بدون التحقق من وقت الانتهاء)
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null) return default; 

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            // 2. التحقق من وجود المستخدم والتوكن في الداتابيز
            if (user == null) return default;

            var storedRefreshToken = user.RefreshTokens?.SingleOrDefault(t => t.Token == refreshToken);

            // 3. التحقق من أمان التوكن (هل هي Active؟ هل انتهت؟)
            if (storedRefreshToken == null || !storedRefreshToken.IsActive)
                return default;

            // 4. "حرق" التوكن القديمة (Revoke) لضمان عدم استخدامها مرة أخرى
            storedRefreshToken.RevokedOn = DateTime.Now;

            // 5. توليد طقم توكنات جديد
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var result = await GenerateJwtTokenAsync(user.Id, user.UserName, user.Email, roles);

            return (result.AccessToken, result.RefreshToken, result.ExpiresAt);
        }

        // ميثود سرية لاستخراج البيانات من التوكن حتى لو منتهية
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecritKey)),
                ValidateLifetime = false // أهم سطر: لا تدقق في وقت الانتهاء
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
                CreatedOn = DateTime.Now
            };
        }
    }
}
