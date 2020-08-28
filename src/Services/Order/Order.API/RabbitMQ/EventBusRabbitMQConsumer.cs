using System.Text;
using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Order.Application.Commands;
using Order.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Order.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _rabbitMqConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public EventBusRabbitMQConsumer(IRabbitMQConnection rabbitMqConnection, IMediator mediator, IMapper mapper, IOrderRepository orderRepository)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _mediator = mediator;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        // Reading and consuming basket checkout queue
        public void Consume()
        {
            var channel = _rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.CheckoutQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;
            channel.BasicConsume(queue: EventBusConstants.CheckoutQueue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.CheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<CheckoutEvent>(message);

                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _rabbitMqConnection.Dispose();
        }
    }
}
