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

                switch (error)
                {
                    case CustomException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
#if DEBUG
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
#else
                  var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
#endif
            }
        }
    }
}
