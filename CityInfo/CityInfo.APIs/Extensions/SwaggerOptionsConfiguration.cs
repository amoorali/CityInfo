using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CityInfo.APIs.Extensions
{
    public class SwaggerOptionsConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        #region [ Fields ]
        private readonly IApiVersionDescriptionProvider _provider;
        #endregion

        #region [ Constructure ]
        public SwaggerOptionsConfiguration(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        #endregion

        #region [ Methods ]
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            options.AddSecurityDefinition("CityInfoApiBearerAuth", CreateSecurityScheme());
            options.AddSecurityRequirement(CreateSecurityRequirement());
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }
        #endregion

        #region [ Private ]
        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var version = description.ApiVersion.ToString();

            var info = new OpenApiInfo()
            {
                Title = $"CityInfo.APIs {version}",
                Version = version
            };

            if (description.IsDeprecated)
            {
                info.Description += "This API version has been deprecated.";
            }

            return info;
        }

        private static OpenApiSecurityScheme CreateSecurityScheme()
        {
            return new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the bearer scheme.\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            };
        }

        private static OpenApiSecurityRequirement CreateSecurityRequirement()
        {
            return new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "CityInfoApiBearerAuth"
                        }
                    },
                    new List<string>()
                }
            };
        }
        #endregion
    }
}