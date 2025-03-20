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
    public class TweetRepository : Repository<Tweet>, ITweetRepository
    {
        public TweetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tweet>> GetTweetsByCoinIdAsync(int coinId, int limit = 100)
        {
            return await _dbSet
                .Where(t => t.CoinId == coinId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}
