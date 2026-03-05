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

                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString);
                });

                builder.Services.AddHangfireServices(connectionString);
                builder.Services.AddIdentityServices(builder.Configuration);
                builder.Services.AddSwaggerDocumentation();
                builder.Services.AddCorsPolicies();
                builder.Services.AddCustomRateLimiting();

                builder.Services.AddPersistenceDependencies();
                builder.Services.AddApplicationDependencies();
                builder.Services.AddInfrastructureDependencies(builder.Configuration);

                builder.Services.AddControllers();

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

                app.UseRouting();

                app.UseAuthentication();
                app.UseRateLimiter();
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
//{
//    "emailOrUserName": "dr.ahmed@clinic.com",
//  "password": "Doctor@123"
//}