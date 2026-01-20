namespace Clinic_System.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        #region OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetSection("constr").Value;

            optionsBuilder.UseSqlServer(connectionString);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Global Query Filter for Soft Delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //modelBuilder.Seed();
        }

        public override int SaveChanges()
        {
            ApplyAuditFields();
            //ApplySoftDelete();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditFields();
            //ApplySoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Automatically set CreatedAt and UpdatedAt for entities implementing IAuditable
        /// </summary>
        private void ApplyAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentTime = DateTime.Now;

            foreach (var entry in entries)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    // Set CreatedAt only when adding new entity
                    entity.CreatedAt = currentTime;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Set UpdatedAt when modifying existing entity
                    entity.UpdatedAt = currentTime;
                    
                    // Prevent CreatedAt from being changed
                    entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                }
            }
        }

        //private void ApplySoftDelete()
        //{

        //    var entries = ChangeTracker.Entries()
        //        .Where(e => e.Entity is ISoftDelete && e.State == EntityState.Modified);

        //    var currentTime = DateTime.Now;

        //    foreach (var entry in entries)
        //    {
        //        var entity = (ISoftDelete)entry.Entity;
        //        entry.State = EntityState.Modified;
        //        entity.IsDeleted = true;
        //        entity.DeletedAt = currentTime;
        //    }
        //}
    }
}