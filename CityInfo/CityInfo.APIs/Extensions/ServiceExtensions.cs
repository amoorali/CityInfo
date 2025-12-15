using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace CityInfo.APIs.Extensions
{
    public static class ServiceExtensions
    {
        #region [ API Versioning ]
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            var apiVersionBuilder = services.AddApiVersioning(setupAction =>
            {
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
            });

            apiVersionBuilder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstitutionFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
        #endregion

        #region [ Swagger ]
        public static void ConfigureVersionedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.ConfigureOptions<SwaggerOptionsConfiguration>();
        }

        public static void UseSwaggerWithVersioning(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var apiVersionDescriptionProvider = 
                    app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            $"CityInfo.API {description.GroupName.ToUpperInvariant()}");
                    }
                });
            }
        }
        #endregion

        #region [ Authentication ]
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Authentication:Issuer"],
                        ValidAudience = configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(configuration["Authentication:SecretForKey"]))
                    };
                });
        }
        #endregion

        #region [ Authorization ]
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeFromAntwerp", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("city", "Antwerp");
                });
            });
        }
        #endregion

        #region [ Other Services ]
        public static void ConfigureServices(this IServiceCollection services)
        {
            #region [ Problem Details ]
            services.AddProblemDetails();

            //builder.Services.AddProblemDetails(options =>
            //{
            //    options.CustomizeProblemDetails = ctx =>
            //    {
            //        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional info example");
            //        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
            //    };
            //});
            #endregion

            services.AddSingleton<FileExtensionContentTypeProvider>();
        }
        #endregion
    }
}
