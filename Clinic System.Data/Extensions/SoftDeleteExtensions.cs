namespace Clinic_System.Data.Extensions
{
    public static class SoftDeleteExtensions
    {

        /// <summary>
        /// Include deleted records in the query (ignore soft delete filter)
        /// </summary>
        public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query) where T : class, ISoftDelete
        {
            // الحل: IgnoreQueryFilters() كافي - لا حاجة لـ Where condition
            return query.IgnoreQueryFilters();
        }

        /// <summary>
        /// Get only deleted records
        /// </summary>
        public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) where T : class, ISoftDelete
        {
            return query.IgnoreQueryFilters().Where(x => x.IsDeleted);
        }

        /// <summary>
        /// Get only non-deleted records (default behavior, but explicit)
        /// </summary>
        public static IQueryable<T> NotDeleted<T>(this IQueryable<T> query) where T : class, ISoftDelete
        {
            return query.Where(x => !x.IsDeleted);
        }

        /// <summary>
        /// Soft delete an entity (uses Egypt timezone)
        /// </summary>
        public static void SoftDelete<T>(this DbSet<T> dbSet, T entity) where T : class, ISoftDelete
        {
            entity.IsDeleted = true;
            entity.DeletedAt = EgyptTimeHelper.GetEgyptTime();
            dbSet.Update(entity);
        }

        /// <summary>
        /// Restore a soft deleted entity
        /// </summary>
        public static void Restore<T>(this DbSet<T> dbSet, T entity) where T : class, ISoftDelete
        {
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            dbSet.Update(entity);
        }

        /// <summary>
        /// Hard delete an entity (permanent delete)
        /// </summary>
        public static void HardDelete<T>(this DbSet<T> dbSet, T entity) where T : class, ISoftDelete
        {
            dbSet.Remove(entity);
        }
    }
}

