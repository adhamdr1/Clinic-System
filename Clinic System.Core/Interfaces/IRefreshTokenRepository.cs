namespace Clinic_System.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        // ميثود متخصصة للحذف الجماعي السريع
        Task DeleteExpiredTokensAsync();
    }
}
