using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraRabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
        }

        public IModel CreateModel()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMQPersistentConnection));

            if (!_connection.IsOpen)
            {
                _connection.Dispose();
                _connection = _connectionFactory.CreateConnection();
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _connection.Dispose();
        }
    }
}
