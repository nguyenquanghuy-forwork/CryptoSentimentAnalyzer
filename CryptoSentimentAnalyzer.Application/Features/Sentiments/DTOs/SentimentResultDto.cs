using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Sentiments.DTOs
{
    public class SentimentResultDto
    {
        public int Id { get; set; }
        public int CoinId { get; set; }
        public string CoinSymbol { get; set; }
        public SentimentType Sentiment { get; set; }
        public double Score { get; set; }
        public DateTime AnalyzedAt { get; set; }
    }
}
