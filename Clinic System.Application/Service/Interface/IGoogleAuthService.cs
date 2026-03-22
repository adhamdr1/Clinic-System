namespace Clinic_System.Application.Service.Interface
{
    public class GoogleUserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo> VerifyGoogleTokenAsync(string idToken);
    }
}
