namespace Clinic_System.Infrastructure.Services
{
    public class RefreshTokenCleanupService : IRefreshTokenCleanupService
    {
        private readonly IUnitOfWork unitOfWork;

        public RefreshTokenCleanupService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task RemoveExpiredRefreshTokensAsync()
        {
            await unitOfWork.RefreshTokensRepository.DeleteExpiredTokensAsync();
        }
    }
}
