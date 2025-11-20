namespace Clinic_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string corsPolicyName = "AllowAll";

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option => {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "My First API",
                    Version = "v1",
                    Description = "This is my first ASP.NET Core Web API",
                    TermsOfService = new Uri("https://www.linkedin.com/in/adham-mohamed74/"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Adham Mohamed",
                        Email = "adhamdr10@gmail.com"
                    }
                });


                option.EnableAnnotations(); // to use Swagger Annotations
            }
            );

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetSection("constr").Value;
                options.UseLazyLoadingProxies().UseSqlServer(connectionString);
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //Check JWT Token Header Replace Cookie
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //return unauthrizre();
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op => {
                op.SaveToken = true;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecritKey"]))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(corsPolicyName);
            app.MapControllers();

            app.Run();
        }
    }
}
