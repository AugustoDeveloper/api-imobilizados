using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Imobilizados.Infrastructure.RabbitMQ.Publisher
{
    public class RabbitMQPublisher
    {
        private bool initialized;
        private readonly ConnectionFactory factory;
        private readonly RabbitMQClientConfiguration configuration;
        public RabbitMQPublisher(RabbitMQClientConfiguration configuration)
        {
            factory = new ConnectionFactory
            {
                HostName = configuration.Hostname,
                VirtualHost = configuration.VirtualHost,
                Port = configuration.Port,
                Password = configuration.Password,
                UserName = configuration.Username
            };

            this.configuration = configuration;
        }

        public void Initialize(IModel channel)
        {
            channel.ExchangeDeclare(
                    exchange: configuration.ExchangeName,
                    type: "direct",
                    durable: true,
                    autoDelete: false,
                    arguments: null
            );

            channel.QueueDeclare(
                queue: configuration.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            channel.QueueBind(
                queue: configuration.QueueName,
                exchange: configuration.ExchangeName,
                routingKey: configuration.RoutingKey
            );

            initialized = true;
        }

        public void Publish(string json)
        {
            using (var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                if (initialized == false)
                {
                    Initialize(channel);
                }

                channel.BasicPublish(
                    exchange: configuration.ExchangeName, 
                    routingKey: configuration.RoutingKey, 
                    mandatory: true,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(json)
                );
            }
        }
    }
}