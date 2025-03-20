using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Infrastructure.Services
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly ILogger<SentimentAnalysisService> _logger;
        private readonly Dictionary<string, int> _positiveWords;
        private readonly Dictionary<string, int> _negativeWords;

        public SentimentAnalysisService(ILogger<SentimentAnalysisService> logger)
        {
            _logger = logger;

            // Simple sentiment word lists - in a real application, you'd use a more comprehensive approach
            _positiveWords = new Dictionary<string, int>
            {
                {"bullish", 2}, {"up", 1}, {"moon", 2}, {"buy", 1}, {"rise", 1}, {"rising", 1},
                {"growth", 1}, {"gain", 1}, {"gains", 1}, {"increasing", 1}, {"good", 1},
                {"great", 1}, {"excellent", 2}, {"profit", 1}, {"opportunity", 1}, {"positive", 1},
                {"success", 1}, {"pump", 1}, {"rally", 2}, {"surge", 2}, {"confident", 1},
                {"strong", 1}, {"strength", 1}, {"uptrend", 2}, {"breakout", 2}, {"win", 1},
                {"winner", 1}, {"winning", 1}, {"potential", 1}, {"promising", 1}, {"confidence", 1}
            };

            _negativeWords = new Dictionary<string, int>
            {
                {"bearish", 2}, {"down", 1}, {"crash", 2}, {"sell", 1}, {"fall", 1}, {"falling", 1},
                {"drop", 1}, {"loss", 1}, {"losses", 1}, {"decreasing", 1}, {"bad", 1},
                {"terrible", 2}, {"poor", 1}, {"risk", 1}, {"risky", 1}, {"negative", 1},
                {"failure", 1}, {"dump", 1}, {"decline", 1}, {"tank", 2}, {"uncertain", 1},
                {"weak", 1}, {"weakness", 1}, {"downtrend", 2}, {"breakdown", 2}, {"lose", 1},
                {"loser", 1}, {"losing", 1}, {"trouble", 1}, {"disappointing", 1}, {"fear", 1}
            };
        }

        public Task<SentimentType> AnalyzeTextAsync(string text)
        {
            var score = CalculateSentimentScore(text);

            SentimentType sentiment = score switch
            {
                > 0.1 => SentimentType.Positive,
                < -0.1 => SentimentType.Negative,
                _ => SentimentType.Neutral
            };

            return Task.FromResult(sentiment);
        }

        public Task<double> GetSentimentScoreAsync(string text)
        {
            var score = CalculateSentimentScore(text);
            return Task.FromResult(score);
        }

        private double CalculateSentimentScore(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            // Normalize text
            text = text.ToLower();
            // Remove URLs, mentions, etc.
            text = Regex.Replace(text, @"https?://\S+|www\.\S+|@\S+|#\S+", "");

            // Split into words
            var words = Regex.Split(text, @"\W+").Where(w => !string.IsNullOrWhiteSpace(w)).ToList();

            double totalScore = 0;
            int totalWeight = 0;

            foreach (var word in words)
            {
                if (_positiveWords.TryGetValue(word, out int positiveWeight))
                {
                    totalScore += positiveWeight;
                    totalWeight += positiveWeight;
                }
                else if (_negativeWords.TryGetValue(word, out int negativeWeight))
                {
                    totalScore -= negativeWeight;
                    totalWeight += negativeWeight;
                }
            }

            // Normalize score between -1 and 1
            if (totalWeight > 0)
            {
                return totalScore / totalWeight;
            }

            return 0;
        }
    }
}
