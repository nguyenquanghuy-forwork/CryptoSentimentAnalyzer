using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Coins.DTOs
{
    public class TweetDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CoinId { get; set; }
        public string CoinSymbol { get; set; }
        public string Sentiment { get; set; }
        public double SentimentScore { get; set; }
    }
}
