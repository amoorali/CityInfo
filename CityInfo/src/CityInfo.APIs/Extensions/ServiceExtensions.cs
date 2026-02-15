using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CityInfo.Application.Behaviors;
using CityInfo.Application.Common.Exceptions;
using CityInfo.Application.Validation.PointOfInterest;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using System.Reflection;

namespace CityInfo.APIs.Extensions
{
    public static class ServiceExtensions
    {
        #region [ Output Formatters ]
        public static void ConfigureOutputFormatters(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                newtonsoftJsonOutputFormatter?.SupportedMediaTypes
                    .Add("application/vnd.marvin.hateoas+json");
            });
        }
        #endregion

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
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<CityInfoContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

            //services.AuthenticateJwtBearer(configuration);
            services.AuthenticateCookie(configuration);
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

        #region [ ExceptionHandler ]
        public static void ExceptionHandlerConfiguration(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                StatusCodeSelector = ex => ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                }
            });
        }
        #endregion

        #region [ Response Caching ]
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddMemoryCache();
        }
        #endregion

        #region [ Cache Headers ]
        public static void ConfigureCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(
                (expirationModelOptions) =>
                {
                    expirationModelOptions.MaxAge = 60;
                    expirationModelOptions.CacheLocation =
                        Marvin.Cache.Headers.CacheLocation.Private;
                },
                (validationModelOptions) =>
                {
                    validationModelOptions.MustRevalidate = true;
                });
        }
        #endregion

        #region [ Other Services ]
        public static void ConfigureServices(this IServiceCollection services)
        {
            #region [ Problem Details ]
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    
                    var problem = context.ProblemDetails;

                    problem.Instance =
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                    if (problem is HttpValidationProblemDetails)
                    {
                        problem.Type = "Model Validation Problem";
                        problem.Status = StatusCodes.Status422UnprocessableEntity;
                        problem.Title = "One or more validation errors occured.";
                        problem.Detail = "See the errors field for details";
                    }

                    if (context.Exception is ValidationException validationException)
                    {
                        problem.Extensions["errors"] = validationException.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray()
                            );
                    }

                    if (context.Exception is NotFoundException notFound)
                    {
                        problem.Status = StatusCodes.Status404NotFound;
                        problem.Title = "Not Found";
                        problem.Detail = notFound.Message;
                    }

                    if (context.Exception is BadRequestException badRequest)
                    {
                        problem.Status = StatusCodes.Status400BadRequest;
                        problem.Title = "Bad Request";
                        problem.Detail = badRequest.Message;
                    }
                };
            });
            #endregion

            #region [ FluentValidation ]
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<PointOfInterestForUpdateDtoValidator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion

            services.AddSingleton<FileExtensionContentTypeProvider>();
        }
        #endregion
    }
}
