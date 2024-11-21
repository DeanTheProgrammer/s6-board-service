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
                        .WriteTo.Console()
                        .WriteTo.Sentry(o =>
                        {
                            o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                            o.MinimumEventLevel = LogEventLevel.Error;
                            o.Dsn = "http://13145c9001224ae8b9c6e673ed4ad6d5@localhost:9000/2";
                            o.SendDefaultPii = true;
                        });
                }
            );
            return hostBuilder;
        }
        public static IHostBuilder UseSentry(this IHostBuilder builder) =>
            builder.ConfigureLogging((context, logging) =>
            {
                var section = context.Configuration.GetRequiredSection("Sentry");
                logging.Services.Configure<SentryLoggingOptions>(section);
                logging.AddSentry();
            });
    }
}
