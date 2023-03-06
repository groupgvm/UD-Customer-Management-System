
/**
 * This attribute is not used. Another middleware has been  introduced  for authentication.
 * 
 * 
 * **/

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CM.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiRequestAuthenticatorAttribute : Attribute, IAsyncActionFilter
    {
        private IConfiguration? configuration;
        private const string API_KEY_HEADER_NAME = "AUTHENTICATION_API_KEY";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // If the relevant header is not found, return 401
            if(!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKeyHeaderValue)) {               
               throw  new UnauthorizedAccessException("Requires an Api Key.");
            }

            configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var trueApiKey = configuration.GetSection(API_KEY_HEADER_NAME);
            if (! trueApiKey.Value.Equals(extractedApiKeyHeaderValue)) {             
               throw new UnauthorizedAccessException("An invalid API key.");
            }

            await next();
        }

       
    }
}