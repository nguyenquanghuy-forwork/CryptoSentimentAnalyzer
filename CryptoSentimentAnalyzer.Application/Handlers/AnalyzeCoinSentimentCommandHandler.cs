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
    public class AnalyzeCoinSentimentCommandHandler : IRequestHandler<AnalyzeCoinSentimentCommand, bool>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ITweetRepository _tweetRepository;
        private readonly ISentimentResultRepository _resultRepository;
        private readonly ITwitterService _twitterService;
        private readonly ISentimentAnalysisService _sentimentService;
        private readonly IMediator _mediator;
        private readonly ILogger<AnalyzeCoinSentimentCommandHandler> _logger;

        public AnalyzeCoinSentimentCommandHandler(
            ICoinRepository coinRepository,
            ITweetRepository tweetRepository,
            ISentimentResultRepository resultRepository,
            ITwitterService twitterService,
            ISentimentAnalysisService sentimentService,
            IMediator mediator,
            ILogger<AnalyzeCoinSentimentCommandHandler> logger)
        {
            _coinRepository = coinRepository;
            _tweetRepository = tweetRepository;
            _resultRepository = resultRepository;
            _twitterService = twitterService;
            _sentimentService = sentimentService;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(AnalyzeCoinSentimentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get coin by symbol
                var coin = await _coinRepository.GetCoinBySymbolAsync(request.CoinSymbol);
                if (coin == null)
                {
                    _logger.LogWarning("Coin with symbol {Symbol} not found", request.CoinSymbol);
                    return false;
                }

                // Fetch tweets for the coin
                var tweets = await _twitterService.FetchTweetsForCoinAsync(coin, request.TweetCount);
                if (!tweets.Any())
                {
                    _logger.LogWarning("No tweets found for coin {Symbol}", request.CoinSymbol);
                    return false;
                }

                // Save tweets to database
                foreach (var tweet in tweets)
                {
                    await _tweetRepository.AddAsync(tweet);
                }
                await _tweetRepository.SaveChangesAsync();

                // Analyze sentiment for each tweet
                foreach (var tweet in tweets)
                {
                    var sentiment = await _sentimentService.AnalyzeTextAsync(tweet.Text);
                    var score = await _sentimentService.GetSentimentScoreAsync(tweet.Text);

                    var sentimentResult = new SentimentResult
                    {
                        CoinId = coin.Id,
                        TweetId = tweet.Id,
                        Sentiment = sentiment,
                        Score = score,
                        AnalyzedAt = DateTime.UtcNow
                    };

                    await _resultRepository.AddAsync(sentimentResult);
                }
                await _resultRepository.SaveChangesAsync();

                // Calculate summary
                await _mediator.Send(new CalculateSentimentSummaryCommand(coin.Id), cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing sentiment for coin {Symbol}", request.CoinSymbol);
                return false;
            }
        }
    }
}
