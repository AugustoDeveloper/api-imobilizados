using System;
using Microsoft.Extensions.Logging;
using Imobilizados.Infrastructure.RabbitMQ;
using Imobilizados.Infrastructure.RabbitMQ.Publish;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Imobilizados.Infrastructure.Logging
{
    public static class RabbitMQLoggerExtension
    {
        public static void AddRabbitMQ(this ILoggerFactory factory, IConfiguration configuration)
        {
            ILoggerProvider provider = new RabbitMQLoggerProvider((n, l, e, ex) => l >= LogLevel.Information, configuration);
            factory.AddProvider(provider);
        }

        public static void AddRabbitMQ(this ILoggerFactory factory, IConfiguration configuration, EventId eventId)
        {
            ILoggerProvider provider = new RabbitMQLoggerProvider(
                (n, l, e, ex) => 
            {
                return l >= LogLevel.Information &&
                    e.Id == eventId;
            }, configuration);
            factory.AddProvider(provider);
        }
    }  
}