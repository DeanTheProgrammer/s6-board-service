using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraRabbitMQ.Object
{
    public class RabbitMQMessageObject
    {
        public string UserId { get; set; }
        public RabbitMQMessageCrudEnum Crud { get; set; }
        public string ObjectName { get; set; }
        public object Object { get; set; }
    }
}
