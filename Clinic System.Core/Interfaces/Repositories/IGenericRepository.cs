namespace Clinic_System.Core.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedAsync(
             int pageNumber,
             int pageSize,
             Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        public Task<IEnumerable<TEntity>> GetAllAsync();

        public Task<TEntity?> GetByIdAsync(int id);

        public Task AddAsync(TEntity entity);

        public void Update(TEntity entity);

        public void Delete(TEntity entity);
        public Task SoftDeleteAsync(int id);

        public Task<TEntity?> GetByCondition(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null);
    }
}
