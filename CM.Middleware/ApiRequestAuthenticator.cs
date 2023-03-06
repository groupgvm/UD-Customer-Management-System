
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CM.Middleware
{
    public class ApiRequestAuthenticator
    {
        private readonly RequestDelegate _next;
        private IConfiguration? configuration;
        private const string API_KEY_HEADER_NAME = "AUTHENTICATION_API_KEY";

        public ApiRequestAuthenticator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // If the relevant header is not found, return 401
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKeyHeaderValue))
            {
                throw new UnauthorizedAccessException("Requires an Api Key.");
            }

            configuration = context.RequestServices.GetService<IConfiguration>();
            var trueApiKey = configuration.GetSection(API_KEY_HEADER_NAME);
            if (!trueApiKey.Value.Equals(extractedApiKeyHeaderValue))
            {
                throw new UnauthorizedAccessException("An invalid API key.");
            }

            await _next(context);
        }
    }

    public static class RequestAuthenticatorExtensions
    {
        public static IApplicationBuilder UseApiRequestAuthenticator(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRequestAuthenticator>();
        }
    }

}