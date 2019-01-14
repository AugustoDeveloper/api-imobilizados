using System;
using Microsoft.Extensions.Configuration;

namespace Imobilizados.Infrastructure.RabbitMQ
{
    public class RabbitMQClientConfiguration
    {
        public const string SectionName = "RabbitConfiguration";
        public string RoutingKey { get; }
        public string QueueName { get; }
        public string ExchangeName { get; }
        public string VirtualHost { get; }
        public string Hostname { get; }
        public int Port { get; }
        public string Username { get; }
        public string Password { get; }

        public RabbitMQClientConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            RoutingKey = configuration[$"{SectionName}:routingKey"];
            QueueName = configuration[$"{SectionName}:queueName"];
            ExchangeName = configuration[$"{SectionName}:exchangeName"];
            VirtualHost = configuration[$"{SectionName}:virtualHost"];
            Hostname = configuration[$"{SectionName}:hostname"];
            Username = configuration[$"{SectionName}:username"];
            Password = configuration[$"{SectionName}:password"];
            string port = configuration[$"{SectionName}:port"];

            if (string.IsNullOrWhiteSpace(RoutingKey))
            {
                throw new ArgumentException(nameof(RoutingKey));
            }

            if (string.IsNullOrWhiteSpace(QueueName))
            {
                throw new ArgumentException(nameof(QueueName));
            }

            if (string.IsNullOrWhiteSpace(ExchangeName))
            {
                throw new ArgumentException(nameof(ExchangeName));
            }

            if (string.IsNullOrWhiteSpace(VirtualHost))
            {
                throw new ArgumentException(nameof(VirtualHost));
            }

            if (string.IsNullOrWhiteSpace(Hostname))
            {
                throw new ArgumentException(nameof(Hostname));
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new ArgumentException(nameof(Username));
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentException(nameof(Password));
            }

            if (int.TryParse(port, out int rabbitPort) == false)
            {
                throw new ArgumentException(nameof(Port));
            }

            Port = rabbitPort;
        }
    }
}