namespace Board_service.Handler.StartupHandler
{
    public class CustomConfigurationBuilder
    {
        public static IConfiguration BuildConfiguration(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .AddCommandLine(args)
                .Build();

            return configuration;
        }
    }
}
