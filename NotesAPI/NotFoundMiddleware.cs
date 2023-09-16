
using Newtonsoft.Json;
using System.Net;

namespace NotesAPI
{
    internal class NotFoundMiddleware
    {

        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 404)
                {
                    // Handle the 404 error here
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"message\": \"Resource not found\"}");
                }

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            };

            // You can include more details in the errorResponse, like the exception message or stack trace

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }

        public class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    
    
    }
}