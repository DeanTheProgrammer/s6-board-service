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
    public class ProjectSyncHandler
    {
        public readonly string QueueName;
        public readonly string ExchangeName = "ProjectSyncExchange";
        public readonly string ApplicationConnectionName;
        public readonly IModel _Channel;
        public readonly ConnectionFactory _factory;
        public ProjectSyncHandler(IOptions<RabbitMQSettings> options)
        {
            RabbitMQSettings settings = options.Value;
            if(settings == null)
            {
                throw new Exception("Settings are not found");
            }
            ApplicationConnectionName = Environment.MachineName + "_" + Guid.NewGuid().ToString();

            _factory = new ConnectionFactory()
            {
                HostName = settings.Hostname,
                UserName = settings.Username,
                Password = settings.Password,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                ClientProvidedName = ApplicationConnectionName
            };


            QueueName = "ProjectSyncQueue_" + ApplicationConnectionName;

            using var connection = _factory.CreateConnection();
            _Channel = connection.CreateModel();

            _Channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, true);
            _Channel.QueueDeclare(QueueName, durable: false, exclusive: false, autoDelete: true, null);
            _Channel.QueueBind(QueueName, ExchangeName, "", null);

            _Channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += ConsumerOnReceived;
            _Channel.BasicConsume(QueueName, false, consumer);

        }

        private void ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
        {
            Thread.Sleep(1000);   
            byte[] body = e.Body.ToArray();
            var JsonProject = Encoding.UTF8.GetString(body);

            ProjectDTO Project = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectDTO>(JsonProject);

            Console.WriteLine("Received object");

            _Channel.BasicAck(e.DeliveryTag, false);
        }

        public void PublishSyncObject(ProjectDTO Project)
        {
            var JsonProject = Newtonsoft.Json.JsonConvert.SerializeObject(Project);
            var body = Encoding.UTF8.GetBytes(JsonProject); 
            _Channel.BasicPublish(ExchangeName, "", null, body);
        }

        public void Dispose()
        {
            _Channel.Dispose();
        }




    }
}
