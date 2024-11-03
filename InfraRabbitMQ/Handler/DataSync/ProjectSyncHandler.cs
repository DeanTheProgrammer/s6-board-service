using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace InfraRabbitMQ.Handler.DataSync
{
    public class ProjectSyncHandler
    {
        public readonly ConnectionFactory _factory;
        public ProjectSyncHandler(IOptions<RabbitMQSettings> options)
        {
            RabbitMQSettings settings = options.Value;
            if(settings == null)
            {
                throw new Exception("Settings are not found");
            }

            _factory = new ConnectionFactory { HostName = settings.HostName };
        }


    }
}
