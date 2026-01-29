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
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IRefreshTokenCleanupService, RefreshTokenCleanupService>();
            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddScoped<IAppointmentNotificationService, AppointmentEmailNotificationService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // بيقرأ القسم من الـ JSON ويربطه بالكلاس
            services.Configure<JwtSettings>(configuration.GetSection("JWT"));
            return services;
        }
    }
}
