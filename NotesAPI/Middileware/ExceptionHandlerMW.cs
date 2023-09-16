using static System.Net.Mime.MediaTypeNames;
using System.Net;

namespace NotesAPI.Middileware
{
    public class ExceptionHandlerMW
    {
        private readonly ILogger<ExceptionHandlerMW> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMW(ILogger<ExceptionHandlerMW> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httcontext)
        {
            try
            {
                await next(httcontext);

            }
            catch (Exception ex)
            {

                //Log this Exception
                var errorid = Guid.NewGuid();
                logger.LogError(ex,$"{errorid} : { ex.Message}");
                //Retiurn a custom error resposne
                httcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httcontext.Response.ContentType = "application/json";

                var errorResponse = new 
                {
                    Id = errorid,
                    ErrorMessage = "Something wrng , We are looking in to this"
                };
                await httcontext.Response.WriteAsJsonAsync( errorResponse );
            }
        }
    }
    }


//RD returns a task that reprensent the completion of request processing