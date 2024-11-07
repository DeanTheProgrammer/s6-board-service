using DTO.DTO_s.Project;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace InfraRabbitMQ.Handler.DataSync
{
    public class ProjectSyncConsumer : BackgroundService
    {
        private readonly RabbitMQPersistentConnection _persistentConnection;
        private IModel _Channel;
        public ProjectSyncConsumer(RabbitMQPersistentConnection persistentConnection)
        {
            _persistentConnection = persistentConnection;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string QueueName = ProjectSyncSettings.QueueName + Environment.MachineName + "_" + Guid.NewGuid().ToString();
            stoppingToken.ThrowIfCancellationRequested();


            _Channel = _persistentConnection.CreateModel();
            _Channel.ExchangeDeclare(ProjectSyncSettings.ExchangeName, ExchangeType.Fanout, true);
            _Channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: true, arguments: null);
            _Channel.QueueBind(QueueName, ProjectSyncSettings.ExchangeName, "", null);
            _Channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_Channel);
            consumer.Received += ConsumerOnReceived;
            _Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
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
    }
}
