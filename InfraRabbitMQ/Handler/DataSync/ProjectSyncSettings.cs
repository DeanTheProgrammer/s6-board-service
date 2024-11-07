using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraRabbitMQ.Handler.DataSync
{
    public static class ProjectSyncSettings
    {
        public const string QueueName = "ProjectSyncQueue_";
        public const string ExchangeName = "ProjectSyncExchange";
    }
}
