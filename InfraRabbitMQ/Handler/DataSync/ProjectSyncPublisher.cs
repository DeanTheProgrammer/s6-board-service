using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DTO.DTO_s.InviteLink;
using RabbitMQ.Client;
using DTO.DTO_s.Project;
using InfraRabbitMQ.Object;
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

        public Task PublishProjectSync(ProjectDTO Project, RabbitMQMessageCrudEnum crudOperation, string userId)
        {

            RabbitMQMessageObject message = new RabbitMQMessageObject
            {
                Crud = crudOperation,
                ObjectName = "Project",
                Object = Project,
                UserId = userId
            };



            var JsonProject = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(JsonProject); 
            _Channel.BasicPublish(ProjectSyncSettings.ExchangeName, "", null, body);
            return Task.CompletedTask;
        }

        public Task PublishInviteLinkSync(InviteLinkDTO InviteLink, RabbitMQMessageCrudEnum crudOpration, string UserId)
        {
            RabbitMQMessageObject message = new RabbitMQMessageObject
            {
                Crud = crudOpration,
                ObjectName = "InviteLink",
                Object = InviteLink,
                UserId = UserId
            };

            var JsonInviteLink = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(JsonInviteLink);
            _Channel.BasicPublish(ProjectSyncSettings.ExchangeName, "", null, body);
            return Task.CompletedTask;
        }
    }
}
