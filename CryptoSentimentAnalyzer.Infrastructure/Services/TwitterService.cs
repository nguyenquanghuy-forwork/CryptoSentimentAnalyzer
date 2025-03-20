using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
namespace CryptoSentimentAnalyzer.Infrastructure.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly ILogger<TwitterService> _logger;
        private readonly TwitterClient _twitterClient;

        public TwitterService(IConfiguration configuration, ILogger<TwitterService> logger)
        {
            _logger = logger;

            // Get Twitter API credentials from configuration
            var consumerKey = configuration["Twitter:ConsumerKey"];
            var consumerSecret = configuration["Twitter:ConsumerSecret"];
            var accessToken = configuration["Twitter:AccessToken"];
            var accessTokenSecret = configuration["Twitter:AccessTokenSecret"];

            // Create Twitter client
            _twitterClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }

        public async Task<IEnumerable<Tweet>> FetchTweetsForCoinAsync(Coin coin, int count = 100)
        {
            try
            {
                var searchParameters = new SearchTweetsParameters($"#{coin.Symbol} OR {coin.Name} crypto")
                {
                    Lang = LanguageFilter.English,
                    SearchType = SearchResultType.Recent,
                    MaxId = count
                };

                var searchResult = await _twitterClient.Search.SearchTweetsAsync(searchParameters);

                var tweets = searchResult.Select(t => new Tweet
                {
                    Id = t.Id,
                    Text = t.Text,
                    Author = t.CreatedBy.ScreenName,
                    CreatedAt = t.CreatedAt.DateTime,
                    CoinId = coin.Id
                }).ToList();

                return tweets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tweets for coin {Symbol}", coin.Symbol);
                return new List<Tweet>();
            }
        }
    }
}
