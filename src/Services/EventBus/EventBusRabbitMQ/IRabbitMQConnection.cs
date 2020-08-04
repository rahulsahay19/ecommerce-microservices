namespace EventBusRabbitMQ
{
    using System;
    using RabbitMQ.Client;
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
