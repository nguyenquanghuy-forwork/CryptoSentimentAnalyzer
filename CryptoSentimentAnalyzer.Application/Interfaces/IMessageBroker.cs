using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface IMessageBroker
    {
        void Publish<T>(string queue, T message);
        void Subscribe<T>(string queue, Action<T> onMessage);
    }
}
