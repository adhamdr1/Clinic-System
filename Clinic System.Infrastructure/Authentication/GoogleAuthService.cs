namespace Clinic_System.Infrastructure.Authentication
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration _configuration;

        public GoogleAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleUserInfo> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    // بنجيب الـ Client ID اللي إنت حطيته في الـ appsettings.json
                    Audience = new List<string>() { _configuration["GoogleAuthSettings:ClientId"] }
                };

                // السطر ده هو اللي بيكلم سيرفرات جوجل ويتأكد إن التذكرة سليمة مش مزورة
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings); // error

                // لو التذكرة سليمة، جوجل بترجعلنا الـ payload مليان داتا، بناخد منها اللي عايزينه
                return new GoogleUserInfo
                {
                    Email = payload.Email,
                    Name = payload.Name
                };
            }
            catch (Exception)
            {
                // لو التذكرة مزورة أو منتهية، جوجل هترمي Exception
                return null;
            }
        }
    }
}
