namespace Clinic_System.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elite Clinic", Version = "v1" });
                c.EnableAnnotations();

                // تعريف شكل زرار القفل (Authorization) في الـ Swagger
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    // الاسم الرسمي للهيدر في بروتوكول HTTP
                    Name = "Authorization",

                    // وصف بسيط ومختصر يطمن المستخدم إنه يحط التوكن بس
                    Description = "Enter your JWT Access Token directly (No need to type 'Bearer').",

                    // مكان التوكن
                    In = ParameterLocation.Header,

                    // التغيير المهم هنا: Http بدلاً من ApiKey
                    Type = SecuritySchemeType.Http,

                    // بنحدد السكيما إنها Bearer
                    Scheme = JwtBearerDefaults.AuthenticationScheme,

                    // مجرد توضيح إن التوكن نوعها JWT
                    BearerFormat = "JWT"
                });

                // تطبيق الحماية على كل الـ Endpoints في الـ Swagger
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                    });
            });

            return services;
        }
    }
}
