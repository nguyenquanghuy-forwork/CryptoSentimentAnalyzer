using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Domain.Entities
{
    public enum SentimentType
    {
        Positive,
        Negative,
        Neutral
    }

    public class SentimentResult
    {
        public int Id { get; set; }
        public int CoinId { get; set; }
        public Coin Coin { get; set; }
        public long? TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public SentimentType Sentiment { get; set; }
        public double Score { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }
}
