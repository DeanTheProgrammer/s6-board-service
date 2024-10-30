using BoardService.Handler;
using BoardService.Interface;
using InfraMongoDB;
using InfraMongoDB.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddProblemDetails();
            services.AddExceptionHandler<Board_service.Handler.ExceptionHandler.ExceptionHandler>();

            services.Configure<MongoDBSettings>(Configuration.GetSection(MongoDBSettings.Settings));


            services.AddScoped<BoardDSInterface, BoardInfrastructure>();
            services.AddScoped<InviteDSInterface, InviteLinkInfrastructure>();
            


            //Service layer
            services.AddScoped<BoardHandler>();
            services.AddScoped<InviteLinkHandler>();



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Board_service", Version = "v1" });
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

            app.UseExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
