namespace Board_service.Handler.StartupHandler
{
    public class CustomConfigurationBuilder
    {
        public static IConfiguration BuildConfiguration(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return configuration;
        }
    }
}
