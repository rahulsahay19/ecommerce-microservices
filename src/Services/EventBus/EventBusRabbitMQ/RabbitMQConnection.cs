namespace EventBusRabbitMQ
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Exceptions;
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQConnection> _logger;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            if (!IsConnected)
            {
                TryConnect();
            }
        }
       public bool IsConnected
        {
            get { return _connection != null && _connection.IsOpen && !_disposed; }
        }
        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException ex)
            {
                // wait for 3 secs and then retry
                Thread.Sleep(3000);
                _connection = _connectionFactory.CreateConnection();
                _logger.LogWarning($"Retrying connection again.");
                
            }

            if (IsConnected)
            {
                return true;
            }
            return false;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                _logger.LogError("No Rabbit MQ connection.");
                throw new ApplicationException("No Rabbit MQ connection.");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while disposing connection:- {ex.Message}");
                throw;
            }
        }
    }
}
