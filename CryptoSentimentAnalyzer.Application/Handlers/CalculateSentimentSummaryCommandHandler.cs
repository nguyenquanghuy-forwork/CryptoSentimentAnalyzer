using CryptoSentimentAnalyzer.Application.Commands;
using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Handlers
{
    public class CalculateSentimentSummaryCommandHandler : IRequestHandler<CalculateSentimentSummaryCommand, bool>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ISentimentResultRepository _resultRepository;
        private readonly ISentimentSummaryRepository _summaryRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<CalculateSentimentSummaryCommandHandler> _logger;

        public CalculateSentimentSummaryCommandHandler(
            ICoinRepository coinRepository,
            ISentimentResultRepository resultRepository,
            ISentimentSummaryRepository summaryRepository,
            IMessageBroker messageBroker,
            ILogger<CalculateSentimentSummaryCommandHandler> logger)
        {
            _coinRepository = coinRepository;
            _resultRepository = resultRepository;
            _summaryRepository = summaryRepository;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task<bool> Handle(CalculateSentimentSummaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get coin
                var coin = await _coinRepository.GetByIdAsync(request.CoinId);
                if (coin == null)
                {
                    _logger.LogWarning("Coin with ID {Id} not found", request.CoinId);
                    return false;
                }

                // Get recent sentiment results (last 24 hours)
                var startDate = DateTime.UtcNow.AddHours(-24);
                var results = await _resultRepository.GetResultsByCoinIdAsync(request.CoinId, startDate);

                if (!results.Any())
                {
                    _logger.LogWarning("No sentiment results found for coin {Symbol}", coin.Symbol);
                    return false;
                }

                // Calculate percentages
                int totalResults = results.Count();
                int positiveCount = results.Count(r => r.Sentiment == SentimentType.Positive);
                int negativeCount = results.Count(r => r.Sentiment == SentimentType.Negative);
                int neutralCount = results.Count(r => r.Sentiment == SentimentType.Neutral);

                double positivePercentage = Math.Round((double)positiveCount / totalResults * 100, 2);
                double negativePercentage = Math.Round((double)negativeCount / totalResults * 100, 2);
                double neutralPercentage = Math.Round((double)neutralCount / totalResults * 100, 2);

                // Create or update summary
                var summary = new SentimentSummary
                {
                    CoinId = coin.Id,
                    PositivePercentage = positivePercentage,
                    NegativePercentage = negativePercentage,
                    NeutralPercentage = neutralPercentage,
                    TotalTweets = totalResults,
                    CalculatedAt = DateTime.UtcNow
                };

                await _summaryRepository.AddAsync(summary);
                await _summaryRepository.SaveChangesAsync();

                // Publish update to subscribers
                _messageBroker.Publish("sentiment-update", new
                {
                    CoinId = coin.Id,
                    CoinSymbol = coin.Symbol,
                    PositivePercentage = positivePercentage,
                    NegativePercentage = negativePercentage,
                    NeutralPercentage = neutralPercentage,
                    TotalTweets = totalResults,
                    CalculatedAt = DateTime.UtcNow
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating sentiment summary for coin ID {Id}", request.CoinId);
                return false;
            }
        }
    }
}
