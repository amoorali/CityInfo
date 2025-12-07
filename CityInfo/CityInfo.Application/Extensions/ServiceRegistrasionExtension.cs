using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CityInfo.Application.Extensions
{
    public static class ServiceRegistrasionExtension
    {
        public static void ConfigureApplicationLayer(this IServiceCollection services)
        {
            #region [ AutoMapper ]
            services.AddAutoMapper(configAction =>
            {

            }, AppDomain.CurrentDomain.GetAssemblies());
            #endregion
        }
    }
}
