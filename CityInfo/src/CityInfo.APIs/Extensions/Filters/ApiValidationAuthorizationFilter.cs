using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CityInfo.APIs.Extensions.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiValidationAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsValidApiKey(context))
                context.Result = new UnauthorizedResult();
        }

        private bool IsValidApiKey(AuthorizationFilterContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var validApiKey = config.GetValue<string>("Authentication:ApiKey");

            if (string.IsNullOrWhiteSpace(validApiKey))
            {
                throw new KeyNotFoundException("ApiKey not found in configuration");
            }

            // try and get the key from a custom request header "X-ApiKey"
            if (!context.HttpContext.Request.Headers.TryGetValue("X-ApiKey", out var extractedKey))
            {
                // as fallback option, try getting it from the query string
                if (!context.HttpContext.Request.Query.TryGetValue("X-ApiKey", out extractedKey))
                {
                    return false; 
                }
            }

            return validApiKey.Equals(extractedKey);
        }
    }
}
