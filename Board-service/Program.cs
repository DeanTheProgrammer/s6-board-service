using System;
using System.Collections.Generic;
using System.Linq;
using Board_service.Handler.StartupHandler;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

namespace Board_service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            AppDomain.CurrentDomain.ProcessExit += Current_ApplicationClosing;
            Log.Information("Service is starting...");
            try
            {
                var host = CustomHostBuilder.CreateHostBuilder(args).Build();
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Service terminated unexpectedly!");
            }
        }

        static async void Current_ApplicationClosing(object sender, EventArgs e)
        {
            await Log.CloseAndFlushAsync();
        }
    }
}