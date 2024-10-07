using Microsoft.AspNetCore.Diagnostics;

namespace Board_service.Handler.ExceptionHandler
{
    public class ExceptionHandler : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            bool ErrorHanled = false;
            Exception ToUse = exception;
            string ResponseMessage = "";

            if (exception.InnerException != null)
            {
                ToUse = exception.InnerException;
            }

            switch (exception)
            {
                case MissingFieldException missingFieldException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    ResponseMessage = "You made a bad request";
                    break;

                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    ResponseMessage = "Internal Server error, Hi Welcome";
                    break;
            }


            if (ResponseMessage != string.Empty)
            {
                await httpContext.Response.WriteAsJsonAsync(ResponseMessage);
                ErrorHanled = true;
            }



            return ErrorHanled;
        }
    }
}
