using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Domain.Entities
{
    public class SentimentSummary
    {
        public int Id { get; set; }
        public int CoinId { get; set; }
        public Coin Coin { get; set; }
        public double PositivePercentage { get; set; }
        public double NegativePercentage { get; set; }
        public double NeutralPercentage { get; set; }
        public int TotalTweets { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
