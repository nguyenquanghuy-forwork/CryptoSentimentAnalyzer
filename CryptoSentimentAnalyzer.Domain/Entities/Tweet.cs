using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Domain.Entities
{
    public class Tweet
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CoinId { get; set; }
        public Coin Coin { get; set; }
        public SentimentResult SentimentResult { get; set; }
    }
}
