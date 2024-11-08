using InfraMongoDB;
using InfraMongoDB.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net;
using Microsoft.Extensions.DependencyInjection;
using ProjectService.Handler;
using ProjectService.Interface;
using InfraRabbitMQ;
using InfraRabbitMQ.Handler.DataSync;
using Microsoft.Extensions.Options;
using ProjectService.DataSyncManagement;
using RabbitMQ.Client;

namespace Board_service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddHttpContextAccessor();
            services.AddProblemDetails();
            services.AddExceptionHandler<Board_service.Handler.ExceptionHandler.ExceptionHandler>();

            //All settings
            services.Configure<MongoDBSettings>(Configuration.GetSection(MongoDBSettings.Settings));
            services.Configure<RabbitMQSettings>(Configuration.GetSection(RabbitMQSettings.Settings));

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
                return new ConnectionFactory()
                {
                    HostName = settings.Hostname,
                    UserName = settings.Username,
                    Password = settings.Password,
                    Port = AmqpTcpEndpoint.UseDefaultPort,
                    ClientProvidedName = Environment.MachineName + "_" + Guid.NewGuid().ToString(),
                    RequestedHeartbeat = TimeSpan.FromSeconds(60)
                };
            });


            services.AddScoped<ProjectDSInterface, ProjectInfrastructure>();
            services.AddScoped<InviteDSInterface, InviteLinkInfrastructure>();

            services.AddScoped<ProjectSyncInterface, ProjectInfrastructure>();



            //Service layer
            services.AddScoped<ProjectHandler>();
            services.AddScoped<InviteLinkHandler>();

            services.AddScoped<ProjectSyncManagment>();

            services.AddSingleton<RabbitMQPersistentConnection>();

            services.AddSingleton<ProjectSyncPublisher>();

            //hosted consumers
            services.AddHostedService<ProjectSyncConsumer>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project service", Version = "v1" });
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthorization();
            //app.UseAntiforgery();

            app.UseExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
