namespace Clinic_System.API.Extensions
{
    public static class SignalRServiceExtensions
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            // السطر ده بيعرف الـ .NET إنه يجهز خدمات الـ Real-Time
            services.AddSignalR();

            return services;
        }
    }
}
