namespace Clinic_System.Application.Service.Interface
{
    public interface IRefreshTokenCleanupService
    {
        Task RemoveExpiredRefreshTokensAsync();
    }
}
