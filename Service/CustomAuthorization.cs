using Azure.Core.Serialization;
using StudentListAPI.Model;
using System.Text.Json;
namespace StudentListAPI.Service
{
    public class CustomAuthorization
    {
        private readonly RequestDelegate _next;

        public CustomAuthorization(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse() { ErrorCode = 403, Error = "Not authorized to perform this action" }));
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse() {ErrorCode=401, Error= "Please log in to access this resource" }));
            }
        }
    }
}
