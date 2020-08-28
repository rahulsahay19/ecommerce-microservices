using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.API.RabbitMQ;

namespace Order.API.Extensions
{
    using Microsoft.AspNetCore.Builder;
    public static class ApplicationBuilderExtensions
    {
        public static EventBusRabbitMQConsumer RabbitMqConsumer { get; set; }
        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder applicationBuilder)
        {
            RabbitMqConsumer = applicationBuilder.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            var lifetime = applicationBuilder.ApplicationServices.GetService<IHostApplicationLifetime>();
            // This way I can gain access to asp.net app lifetime
            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
            return applicationBuilder;
        }

        // When the app starts, it will call the consume/disconnect from middleware
        private static void OnStarted()
        {
            RabbitMqConsumer.Consume();
        }
        private static void OnStopping()
        {
            RabbitMqConsumer.Disconnect();
        }
    }
}
