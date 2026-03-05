namespace Clinic_System.API.Extensions
{
    public static class HangfireServiceExtensions
    {
        public static IServiceCollection AddHangfireServices(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
                config.UseSqlServerStorage(connectionString));

            services.AddHangfireServer(); // تشغيل الـ Server المسؤول عن تنفيذ المهام

            return services;
        }
    }
}