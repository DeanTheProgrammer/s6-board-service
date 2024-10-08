using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

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

            switch (ToUse)
            {
                case MissingFieldException missingFieldException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    ResponseMessage = "You where missing a field";
                    UserException(missingFieldException);
                    break;

                case ValidationException validationException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    ResponseMessage = validationException.Message;
                    UserException(validationException);
                    break;

                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    ResponseMessage = "Internal Server error";
                    SystemException(ToUse);
                    break;
            }


            if (ResponseMessage != string.Empty)
            {
                await httpContext.Response.WriteAsJsonAsync(ResponseMessage);
                ErrorHanled = true;
            }



            return ErrorHanled;
        }

        private void UserException(Exception ex)
        {
            Log.Information("A user generated a Exception", ex);

        }

        private void SystemException(Exception ex)
        {
            Log.Error("A system generated a Exception", ex);
        }
    }
}
