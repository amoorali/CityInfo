using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CityInfo.Application.Extensions
{
    public static class ServiceRegistrasionExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region [ AutoMapper ]
            services.AddAutoMapper(configAction =>
            {

            }, Assembly.GetExecutingAssembly());
            #endregion
        }
    }
}
