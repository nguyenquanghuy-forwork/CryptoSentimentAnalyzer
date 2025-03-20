using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.DTOs
{
    public class SentimentSummaryDto
    {
        public int CoinId { get; set; }
        public string CoinSymbol { get; set; }
        public string CoinName { get; set; }
        public double PositivePercentage { get; set; }
        public double NegativePercentage { get; set; }
        public double NeutralPercentage { get; set; }
        public int TotalTweets { get; set; }
        public DateTime CalculatedAt { get; set; }
        public string OverallSentiment => PositivePercentage > NegativePercentage ? "Bullish" : "Bearish";
    }
}
