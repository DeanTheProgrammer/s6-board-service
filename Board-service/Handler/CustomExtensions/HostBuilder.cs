using Board_service.Handler.CustomExtensions;
using Serilog;
using Serilog.Events;
using Sentry;
using Sentry.Serilog;

namespace Board_service.Handler.StartupHandler
{
    public static class CustomHostBuilder
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args) // Fixed: Added return statement
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Program>();
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        if (context.HostingEnvironment.IsProduction())
                        {
                            options.ListenAnyIP(8081,
                                listenOptions => { listenOptions.UseHttps("certhttps.pfx", "Password123"); });
                            options.ListenAnyIP(8080);
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .AddAppLogging();
        }
    }
}
