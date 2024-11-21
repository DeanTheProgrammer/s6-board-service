using System.Net.Mime;
using Microsoft.Net.Http.Headers;
using Serilog;
using Microsoft.Extensions.Hosting;
using Serilog.Enrichers;
using Serilog.Configuration;
using Serilog.Events;
using Sentry;
using Sentry.Serilog;
using Sentry.Extensions.Logging;
using static System.Net.WebRequestMethods;

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
                        .Enrich.WithProperty("Application", "Board-service")
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Device Name", System.Environment.MachineName)
                        .Enrich.WithClientIp()
                        .Enrich.WithCorrelationId(headerName: "correclation-id", addValueIfHeaderAbsence: true)
                        .Enrich.WithRequestHeader("x-forwarded-for")
                        .Enrich.WithRequestHeader("HTTP_CLIENT_IP")
                        .Enrich.WithRequestHeader(HeaderNames.UserAgent)
                        .WriteTo.Console();
                }
            );
            return hostBuilder;
        }
    }
}
