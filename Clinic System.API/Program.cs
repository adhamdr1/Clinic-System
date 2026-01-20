using Clinic_System.Infrastructure.Authentication.Models;

namespace Clinic_System.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/bootstrap-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            try
            {
                Log.Information("Program Starting");

                var builder = WebApplication.CreateBuilder(args);

                // Serilog
                builder.Host.UseSerilog((context, services, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services);
                });


                var connectionString = builder.Configuration.GetSection("constr").Value;

                // Database Configuration
                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString);
                });

                // إضافة Hangfire Services
                builder.Services.AddHangfire(config =>
                               config.UseSqlServerStorage(connectionString));

                builder.Services.AddHangfireServer(); // تشغيل الـ Server المسؤول عن تنفيذ المهام

                // Identity Configuration
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

                // JWT Authentication
                var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.IssuerIP,
                        ValidAudience = jwtSettings.AudienceIP,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecritKey!))
                        //ClockSkew = TimeSpan.Zero
                    };
                });

                builder.Services.AddControllers();

                // Swagger/OpenAPI Configuration
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elite Clinic", Version = "v1" });
                    c.EnableAnnotations();

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

                // CORS Configuration
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
                });

                builder.Services.AddPersistenceDependencies();
                builder.Services.AddApplicationDependencies();
                builder.Services.AddInfrastructureDependencies(builder.Configuration);

                if (string.IsNullOrWhiteSpace(jwtSettings.SecritKey))
                    throw new Exception("JWT SecretKey is missing in appsettings.json");

                if (string.IsNullOrWhiteSpace(jwtSettings.IssuerIP))
                    throw new Exception("JWT IssuerIP is missing");

                if (string.IsNullOrWhiteSpace(jwtSettings.AudienceIP))
                    throw new Exception("JWT AudienceIP is missing");

                var app = builder.Build();

                app.UseMiddleware<ErrorHandlerMiddleware>();


                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elite Clinic");
                    });
                }

                app.UseHttpsRedirection();


                app.UseCors("AllowAll");

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.UseHangfireDashboard();

                JobScheduler.ScheduleRecurringJobs(app);

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program Stoped");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}