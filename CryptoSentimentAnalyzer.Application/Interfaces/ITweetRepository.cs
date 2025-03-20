using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface ITweetRepository : IRepository<Tweet>
    {
        Task<IEnumerable<Tweet>> GetTweetsByCoinIdAsync(int coinId, int limit = 100);
    }
}
