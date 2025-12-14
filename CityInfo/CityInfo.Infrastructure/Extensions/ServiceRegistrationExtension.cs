using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Repositories.Contracts;
using CityInfo.Infrastructure.Repositories.Implementations;
using CityInfo.Infrastructure.Services.Contracts;
using CityInfo.Infrastructure.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.Infrastructure.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static void ConfigureInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            #region [ Database Connection ]
            services.AddDbContext<CityInfoContext>(options =>
                options.UseSqlite(
                configuration["ConnectionStrings:CityInfoDBConnectionString"]));
            #endregion

            #region [ Repositories ]
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IPointOfInterestRepository, PointOfInterestRepository>();
            #endregion

            #region [ Services ]
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            #region [ UnitOfWork ]
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #endregion
        }
    }
}
