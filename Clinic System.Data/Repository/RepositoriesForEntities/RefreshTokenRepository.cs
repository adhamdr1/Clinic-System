namespace Clinic_System.Data.Repository.RepositoriesForEntities
{
    public class RefreshTokenRepository : GenericRepository<Prescription>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public async Task DeleteExpiredTokensAsync()
        {
            // هنا بنستخدم الـ Bulk Delete المباشر وسريع جداً
            await context.Set<RefreshToken>()
                .Where(t => t.ExpiresOn <= DateTime.Now || t.RevokedOn != null)
                .ExecuteDeleteAsync();
        }
    }
}
