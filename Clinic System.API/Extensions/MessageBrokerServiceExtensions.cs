namespace Clinic_System.API.Extensions
{
    public static class MessageBrokerServiceExtensions
    {
        public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                // 1. تسجيل كل العمال (Consumers)
                x.AddConsumer<AppointmentBookedEventConsumer>();
                x.AddConsumer<AppointmentCancelledEventConsumer>();
                x.AddConsumer<AppointmentRescheduledEventConsumer>();
                x.AddConsumer<AppointmentNoShowEventConsumer>();
                x.AddConsumer<AppointmentConfirmedEventConsumer>();
                x.AddConsumer<MedicalReportGeneratedEventConsumer>();
                x.AddConsumer<AppointmentAutoCancelledEventConsumer>();
                x.AddConsumer<UserRegisteredEventConsumer>();
                x.AddConsumer<PasswordResetRequestedEventConsumer>();

                // 2. إعداد الاتصال بسيرفر RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    // بنسحب اللينك بتاع CloudAMQP
                    var rabbitMqUrl = configuration["RabbitMQ:Url"];

                    if (string.IsNullOrEmpty(rabbitMqUrl))
                        throw new Exception("RabbitMQ URL is missing from User Secrets/Configuration!");

                    // الاتصال بالسيرفر
                    cfg.Host(new Uri(rabbitMqUrl));


                    cfg.UseMessageRetry(r =>
                    {
                        // 1. حاول تاني فقط لو الخطأ له علاقة بالشبكة أو السيرفر (Transient)
                        r.Handle<HttpRequestException>();
                        r.Handle<TimeoutException>();
                        r.Handle<TaskCanceledException>();

                        // 2. إياك تحاول تاني لو الخطأ "منطقي" في الكود (Permanent)
                        r.Ignore<ArgumentNullException>();
                        r.Ignore<InvalidOperationException>();

                        r.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
                    });

                    // تكوين الـ Queues أوتوماتيك
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
