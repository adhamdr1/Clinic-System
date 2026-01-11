namespace Clinic_System.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Email Settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Services
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddScoped<IAppointmentNotificationService, AppointmentEmailNotificationService>();
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
