namespace Clinic_System.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<(string Token, DateTime ExpiresAt, string? userName, string? email, List<string>? Roles)> GenerateJwtTokenAsync(
             string userId, string userName, string email, List<string> roles)
        {
            // 1. تحويل المفتاح السري (الختم) من نص لمفتاح تشفير
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecritKey));

            // 2. تجهيز البيانات (Claims) اللي هتتحفر جوه التوكن
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId), // معرف المستخدم
                new Claim(JwtRegisteredClaimNames.UniqueName, userName), // اسم المستخدم
                new Claim(JwtRegisteredClaimNames.Email, email), // البريد الإلكتروني
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // رقم فريد للتوكن لمنع التزوير
            };

            // إضافة الأدوار (Roles) للـ Claims عشان الـ Authorization تشتغل
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 3. تحديد وقت انتهاء الكارنيه (التوكن)
            var expiresAt = DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes);

            // 4. إنشاء كائن التوكن بكل تفاصيله
            var tokenObject = new JwtSecurityToken(
                issuer: _jwtSettings.IssuerIP,
                audience: _jwtSettings.AudienceIP,
                expires: expiresAt,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 5. تحويل الكائن لسطر نصي طويل (String)
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            // 6. إرجاع الـ Tuple المطلوبة للـ Handler
            return (tokenString, expiresAt, userName, email, roles);
        }
    }
}
