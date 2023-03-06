#define DEBUG
#define RELEASE

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CM.Middleware
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string errorMessage = null;

                switch (error)
                {
                    case CustomException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorMessage = "Bad Request";
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorMessage = "Not Found";
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorMessage = "Unauthorized";
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorMessage = "Server Error";
                        break;
                }
#if DEBUG
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
#else
                var result = JsonSerializer.Serialize(new { message = errorMessage });
                await response.WriteAsync(result);
#endif
            }
        }
    }
}
