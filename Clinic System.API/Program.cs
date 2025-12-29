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

                // Database Configuration
                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    var connectionString = builder.Configuration.GetSection("constr").Value;
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString);
                });
                
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
                var jwtSettings = builder.Configuration.GetSection("JWT");
                var secritKey = jwtSettings["SecritKey"];
                var audience = jwtSettings["AudienceIP"];
                var issuer = jwtSettings["IssuerIP"];
                
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
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secritKey!)),
                    };
                });
                
                builder.Services.AddControllers();
                
                // Swagger/OpenAPI Configuration
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                
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
                
                
                builder.Services.AddAutoMapper(typeof(ApplicationAssemblyReference).Assembly);
                builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyReference).Assembly));
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
                builder.Services.AddScoped<IDoctorService, DoctorService>();
                builder.Services.AddScoped<IPatientService, PatientService>();
                builder.Services.AddScoped<IAppointmentService, AppointmentService>();
                builder.Services.AddScoped<IPaymentService , PaymentService>();
                builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
                builder.Services.AddScoped<IIdentityService, IdentityService>();
                builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
                builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                if (string.IsNullOrWhiteSpace(secritKey))
                    throw new Exception("JWT SecretKey is missing in appsettings.json");

                if (string.IsNullOrWhiteSpace(issuer))
                    throw new Exception("JWT IssuerIP is missing");

                if (string.IsNullOrWhiteSpace(audience))
                    throw new Exception("JWT AudienceIP is missing");

                var app = builder.Build();
                
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("", "Clinic System");
                    });
                }
                
                app.UseHttpsRedirection();
                
                app.UseMiddleware<ErrorHandlerMiddleware>();
                
                app.UseCors("AllowAll");
                
                app.UseAuthentication();
                app.UseAuthorization();
                
                app.MapControllers();

                app.Run(); 
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program Stoped");
            }
            finally
            {
                Log.CloseAndFlush(); // «· √ﬂœ „‰ ﬂ «»… ﬂ· «·»Ì«‰«  ··„·› ﬁ»· «·≈€·«ﬁ
            }
        }
    }
}
