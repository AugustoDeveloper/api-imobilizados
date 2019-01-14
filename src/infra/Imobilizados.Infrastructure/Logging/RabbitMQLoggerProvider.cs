using System;
using Microsoft.Extensions.Logging;
using Imobilizados.Infrastructure.RabbitMQ;
using Imobilizados.Infrastructure.RabbitMQ.Publisher;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Imobilizados.Infrastructure.Logging
{
    public class RabbitMQLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguration configuration;
        private Func<string, LogLevel, EventId, Exception, bool> filter;
        public RabbitMQLoggerProvider(Func<string, LogLevel, EventId, Exception, bool> filter, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.filter = filter;
        }

        public ILogger CreateLogger(string categoryName) => new RabbitMQLogger(categoryName, filter, new RabbitMQClientConfiguration(configuration));

        public void Dispose() { }

        private class RabbitMQLogger : ILogger
        {
            private readonly string categoryName;
            private readonly RabbitMQClientConfiguration configuration;
            private Func<string, LogLevel, EventId, Exception, bool> filter;
            private Func<string, LogLevel, EventId, Exception, bool> Filter 
            {
                get => this.filter;
                set => this.filter = value ?? throw new ArgumentNullException("Filter with invalid value");
            } 
            public RabbitMQLogger(string name, Func<string, LogLevel, EventId, Exception, bool> filter, RabbitMQClientConfiguration configuration)
            {
                categoryName = name;
                this.filter = filter ?? ((category, level, eventId, exception) => true);
                this.configuration = configuration;
            }
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel) => IsEnabled(logLevel, null);

            public bool IsEnabled(LogLevel logLevel, Exception exception) => IsEnabled(logLevel, default(EventId), exception);
            
            public bool IsEnabled(LogLevel logLevel, EventId eventId, Exception exception)
            {
                return Filter.Invoke(categoryName, logLevel, eventId, exception);
            }
            

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (IsEnabled(logLevel, eventId, exception) == false)
                {
                    return;
                }

                if (formatter == null)
                {
                    throw new ArgumentNullException(nameof(formatter));
                }

                var properties = new 
                {
                    Source = categoryName,
                    Host = GetMachineName(),
                    EventId = eventId,
                    Level = logLevel,
                    Message = formatter.Invoke(state, exception)
                };

                string json = JsonConvert.SerializeObject(properties);

                new RabbitMQPublisher(configuration).Publish(json);
            }

            public string GetMachineName()
            {
                return !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("COMPUTERNAME")) ? System.Environment.GetEnvironmentVariable("COMPUTERNAME") : System.Net.Dns.GetHostName();
            }
        }
  }
}