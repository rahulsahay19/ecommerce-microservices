using System;

namespace EventBusRabbitMQ.Producer
{
    using System.Text;
    using Events;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    public class EventBusRabbitMQProducer 
    {
        private readonly IRabbitMQConnection _rabbitMqConnection;

        public EventBusRabbitMQProducer(IRabbitMQConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void PublishCheckout(string queueName, CheckoutEvent publishModel)
        {
            using (var channel = _rabbitMqConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent to RabbitMQ");
                };
                channel.ConfirmSelect();
            }
        }
    }
}
