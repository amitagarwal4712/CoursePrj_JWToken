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

        //This is middle ware level error genmeration when like Authorization or role level access get failed and generated exceptions, which are not hold at controller level
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden) //When user role is allowing to access controller method, exception will be raised
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse() { ErrorCode = 403, Error = "Not authorized to perform this action" }));
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)  //When user tried to access controller method without valid JWT token, exception will be raised
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse() {ErrorCode=401, Error= "Please log in to access this resource" }));
            }
        }
    }
}
