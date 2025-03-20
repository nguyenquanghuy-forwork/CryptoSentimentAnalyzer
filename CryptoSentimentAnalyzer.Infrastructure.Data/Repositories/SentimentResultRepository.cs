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
    public class SentimentResultRepository : Repository<SentimentResult>, ISentimentResultRepository
    {
        public SentimentResultRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SentimentResult>> GetResultsByCoinIdAsync(int coinId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(r => r.CoinId == coinId);

            if (startDate.HasValue)
                query = query.Where(r => r.AnalyzedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.AnalyzedAt <= endDate.Value);

            return await query.OrderByDescending(r => r.AnalyzedAt).ToListAsync();
        }
    }
}
