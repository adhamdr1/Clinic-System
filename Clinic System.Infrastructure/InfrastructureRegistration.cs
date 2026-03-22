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
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            services.Configure<ClinicSettings>(configuration.GetSection("ClinicSettings"));

            // بيقرأ القسم من الـ JSON ويربطه بالكلاس
            services.Configure<JwtSettings>(configuration.GetSection("JWT"));


            var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value;

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                // 2. تظبيط إعدادات الاتصال (زي الـ Retries)
                var redisConfiguration = ConfigurationOptions.Parse(redisConnectionString);
                redisConfiguration.ReconnectRetryPolicy = new ExponentialRetry(500, 2000);

                // 3. تسجيل مأمور السنترال (ConnectionMultiplexer) كـ Singleton
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));
            }

            // 4. تسجيل الـ Cache Service بتاعتنا عشان الـ Application Layer تعرف تشوفها
            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
