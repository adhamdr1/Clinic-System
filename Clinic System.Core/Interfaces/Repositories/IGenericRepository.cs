namespace Clinic_System.Core.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedAsync(
             int pageNumber,
             int pageSize,
             Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        public Task<TEntity?> GetByIdAsync(int id);

        public Task AddAsync(TEntity entity);

        public Task UpdateAsync(TEntity entity); 

        public Task DeleteAsync(TEntity entity);
        public Task SoftDeleteAsync(int id);

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null);
    }
}
