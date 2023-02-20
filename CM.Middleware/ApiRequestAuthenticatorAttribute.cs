
using Microsoft.AspNetCore.Mvc;
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
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content =  "Requires an Api Key."

                };

                return;
            }

            configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var trueApiKey = configuration.GetSection(API_KEY_HEADER_NAME);
            if (! trueApiKey.Value.Equals(extractedApiKeyHeaderValue)) {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "An invalid API key."

                };

                return;
            }

            await next();
        }

       
    }
}