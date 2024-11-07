using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RabbitMQ.Client;
using DTO.DTO_s.Project;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace InfraRabbitMQ.Handler.DataSync
{
    public class ProjectSyncPublisher
    {
        private readonly RabbitMQPersistentConnection _persistentConnection;
        private IModel _Channel;
        public ProjectSyncPublisher(RabbitMQPersistentConnection PersistentConnection)
        {
            _persistentConnection = PersistentConnection;
            _Channel = _persistentConnection.CreateModel();
            _Channel.ExchangeDeclare(ProjectSyncSettings.ExchangeName, ExchangeType.Fanout, true);

        }

        public void PublishSyncObject(ProjectDTO Project)
        {
            var JsonProject = Newtonsoft.Json.JsonConvert.SerializeObject(Project);
            var body = Encoding.UTF8.GetBytes(JsonProject); 
            _Channel.BasicPublish(ProjectSyncSettings.ExchangeName, "", null, body);
        }
    }
}
