using Asp.Versioning;
using CityInfo.APIs.Legacy;
using CityInfo.Application.Services.Contracts;
using CityInfo.Application.Services.Implementations;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Repositories.Contracts;
using CityInfo.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;

namespace CityInfo.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


            var builder = WebApplication.CreateBuilder(args);

            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();
            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            builder.Services.AddProblemDetails();

            //builder.Services.AddProblemDetails(options =>
            //{
            //    options.CustomizeProblemDetails = ctx =>
            //    {
            //        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional info example");
            //        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
            //    };
            //});

            builder.Services.AddSwaggerGen(setupAction =>
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
            builder.Services.AddTransient<IMailService, LocalMailService>();
#else
            builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

            builder.Services.AddSingleton<CitiesDataStore>();
            builder.Services.AddDbContext<CityInfoContext>(
                dbContextOptions => dbContextOptions.UseSqlite(
                    builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

            builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeFromAntwerp", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("city", "Antwerp");
                });
            });

            builder.Services.AddApiVersioning(setupAction =>
            {
                setupAction.ReportApiVersions = true;
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddMvc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
