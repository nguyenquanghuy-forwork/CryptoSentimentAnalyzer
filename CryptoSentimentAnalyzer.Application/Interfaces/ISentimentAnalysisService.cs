using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface ISentimentAnalysisService
    {
        Task<SentimentType> AnalyzeTextAsync(string text);
        Task<double> GetSentimentScoreAsync(string text);
    }
}
