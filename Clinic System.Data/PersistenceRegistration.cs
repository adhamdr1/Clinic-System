namespace Clinic_System.Data
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // أي Repositories إضافية بتتحط هنا
            return services;
        }
    }
}
