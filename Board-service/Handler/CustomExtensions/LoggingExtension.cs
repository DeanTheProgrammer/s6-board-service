using Microsoft.Net.Http.Headers;
using Serilog;
using Microsoft.Extensions.Hosting;
using Serilog.Enrichers;
using Serilog.Configuration;
using Serilog.Events;

namespace Board_service.Handler.CustomExtensions
{
    public static class LoggingExtension
    {
        public static IHostBuilder AddAppLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .Enrich.WithClientIp(headerName: "CF-Connection-IP")
                        .Enrich.WithCorrelationId(headerName: "correclation-id", addValueIfHeaderAbsence: true)
                        .Enrich.WithRequestHeader("x-forwarded-for")
                        .Enrich.WithRequestHeader(HeaderNames.UserAgent)
                        .WriteTo.Console();
                }
            );

            return hostBuilder;
        }
    }
}
