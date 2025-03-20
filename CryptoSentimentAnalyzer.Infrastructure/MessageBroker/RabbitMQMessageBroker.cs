using Autofac.Core;
using CryptoSentimentAnalyzer.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoSentimentAnalyzer.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoSentimentAnalyzer.Infrastructure.MessageBroker
{
    public class RabbitMQMessageBroker : IMessageBroker, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQMessageBroker> _logger;
        private readonly Dictionary<string, List<object>> _handlers = new();

        public RabbitMQMessageBroker(IConfiguration configuration, ILogger<RabbitMQMessageBroker> logger)
        {
            
        }

        public void Publish<T>(string queue, T message)
        {
            
        }

        public void Subscribe<T>(string queue, Action<T> onMessage)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
