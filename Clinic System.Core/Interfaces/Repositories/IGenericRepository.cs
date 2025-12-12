namespace Clinic_System.Core.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedAsync(
             int pageNumber,
             int pageSize,
             Expression<Func<TEntity, bool>>? filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
             CancellationToken cancellationToken = default);

        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        public void Update(TEntity entity, CancellationToken cancellationToken = default);

        public void Delete(TEntity entity, CancellationToken cancellationToken = default);
        public void SoftDelete(TEntity entity, CancellationToken cancellationToken = default);

        public Task<TEntity?> GetByCondition(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null, CancellationToken cancellationToken = default);
    }
}
