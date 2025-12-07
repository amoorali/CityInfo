using CityInfo.APIs.Extensions;
using CityInfo.Application.Extensions;
using CityInfo.Infrastructure.Extensions;

namespace CityInfo.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLoggingSetup();

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = false;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            builder.Services.ConfigureApplicationLayer();
            builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
            builder.Services.ConfigureServices();
            builder.Services.ConfigureApiVersioning();
            builder.Services.ConfigureVersionedSwagger();
            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.ConfigureAuthorization();

            var app = builder.Build();

            app.UseSwaggerWithVersioning(app.Environment);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
