namespace Clinic_System.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext context;
        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            int totalCount = await query.CountAsync();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            // الحل: التحقق من حالة الـ Entity قبل Update
            // هذا يمنع Update غير ضروري ويحسن الأداء
            var entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                context.Set<TEntity>().Update(entity);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
        public async Task SoftDeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null) 
                return;

            // التحقق باستخدام الانترفيس بدل الـ Reflection السحري
            if (entity is ISoftDelete softDeletableEntity)
            {
                softDeletableEntity.IsDeleted = true;
                // الحل: استخدام EgyptTimeHelper بدلاً من DateTime.Now
                softDeletableEntity.DeletedAt = EgyptTimeHelper.GetEgyptTime();

                // مش محتاج تنادي UpdateAsync لأن التعديل حصل على الكائن المتتبع (Tracked)
                // ومجرد ما تعمل SaveChanges في UnitOfWork هيسمع.
                // لكن لو عايز تأكيد:
                Update(entity);
            }
        }

        public async Task<TEntity?> GetByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null)
        {
            if (criteria != null)
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync(criteria);
            }

            return await context.Set<TEntity>().AsNoTracking().CountAsync();
        }
    }
}
