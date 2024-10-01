using Board_service.Handler.CustomExtensions;
using Serilog;
using Serilog.Events;

namespace Board_service.Handler.StartupHandler
{
    public static class CustomHostBuilder
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args) // Fixed: Added return statement
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddUserSecrets<Program>();
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .AddAppLogging();
        }
    }
}
