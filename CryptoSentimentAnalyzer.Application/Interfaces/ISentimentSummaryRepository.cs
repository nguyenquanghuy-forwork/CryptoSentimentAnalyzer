using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface ISentimentSummaryRepository : IRepository<SentimentSummary>
    {
        Task<SentimentSummary> GetLatestSummaryByCoinIdAsync(int coinId);
        Task<IEnumerable<SentimentSummary>> GetSummariesByCoinIdsAsync(IEnumerable<int> coinIds);
    }
}
