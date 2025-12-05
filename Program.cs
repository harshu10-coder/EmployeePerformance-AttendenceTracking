using EmployeePerformance_AttendenceTracking.Data;
using EmployeePerformance_AttendenceTracking.Models;
using EmployeePerformance_AttendenceTracking.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.Tasks;
//using System.Security.Cryptography.Xml;

namespace EmployeePerformance_AttendenceTracking
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Add Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultStrings")));




            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add DI
            builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
                
           //Configure JWt

            var JwtSetting = builder.Configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(JwtSetting["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = JwtSetting["Issuer"],
                    ValidAudience = JwtSetting["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)

                };
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options=>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Employee Performance & Attendance API",
                    Version = "v1",
                    Description= "API documentation for EPATS system"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In=ParameterLocation.Header,
                    Description="Enter JWT Token",
                    Name="Authorization",
                    Type=SecuritySchemeType.Http,
                    Scheme="Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                var usermanager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var rolemanager = service.GetRequiredService<RoleManager<IdentityRole>>();

                await DbSeed.SeedDataAsync(usermanager, rolemanager);
            };
            app.Run();
        }
    }
}
