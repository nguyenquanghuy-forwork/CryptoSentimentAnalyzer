using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Infrastructure.Data.Repositories
{
    public class SentimentSummaryRepository : Repository<SentimentSummary>, ISentimentSummaryRepository
    {
        public SentimentSummaryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<SentimentSummary> GetLatestSummaryByCoinIdAsync(int coinId)
        {
            return await _dbSet
                .Where(s => s.CoinId == coinId)
                .OrderByDescending(s => s.CalculatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SentimentSummary>> GetSummariesByCoinIdsAsync(IEnumerable<int> coinIds)
        {
            var summaries = new List<SentimentSummary>();

            foreach (var coinId in coinIds)
            {
                var latestSummary = await GetLatestSummaryByCoinIdAsync(coinId);
                if (latestSummary != null)
                {
                    summaries.Add(latestSummary);
                }
            }

            return summaries;
        }
    }
}
