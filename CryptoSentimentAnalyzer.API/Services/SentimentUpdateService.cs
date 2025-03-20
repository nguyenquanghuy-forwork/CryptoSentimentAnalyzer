using CryptoSentimentAnalyzer.API.Hubs;
using CryptoSentimentAnalyzer.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using CryptoSentimentAnalyzer.API.Hubs;
using CryptoSentimentAnalyzer.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.API.Services
{
    public class SentimentUpdateService : BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IHubContext<SentimentHub> _hubContext;
        private readonly ILogger<SentimentUpdateService> _logger;

        public SentimentUpdateService(
            IMessageBroker messageBroker,
            IHubContext<SentimentHub> hubContext,
            ILogger<SentimentUpdateService> logger)
        {
            _messageBroker = messageBroker;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageBroker.Subscribe<dynamic>("sentiment-update", async (update) =>
            {
                try
                {
                    // Broadcast to all clients subscribed to this coin
                    await _hubContext.Clients.Group(update.CoinSymbol.ToString())
                        .SendAsync("SentimentUpdated", update, cancellationToken: stoppingToken);

                   // //Also broadcast to all clients
                   //await _hubContext.Clients.All
                   //    .SendAsync("AllSentimentsUpdated", update, cancellationToken: stoppingToken);

                   // _logger.LogInformation("Broadcast sentiment update for {Symbol}", update.CoinSymbol);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error broadcasting sentiment update");
                }
            });

            return Task.CompletedTask;
        }
    }
}
