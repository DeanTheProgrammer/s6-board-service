using BoardService.Handler;
using BoardService.Interface;
using InfraMongoDB;
using InfraMongoDB.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net;

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

            //Database infrastructure layer
            MongoDBSettings MongoDBSettings = new MongoDBSettings();
            Configuration.GetSection("MongoDB").Bind(MongoDBSettings);
            services.AddSingleton<BoardDSInterface>(new BoardInfrastructure(MongoDBSettings));
            services.AddSingleton<InviteDSInterface>(new InviteLinkInfrastructure(MongoDBSettings));
            


            //Service layer
            services.AddSingleton<BoardHandler>();
            services.AddSingleton<InviteLinkHandler>();



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
