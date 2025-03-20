using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface ISentimentResultRepository : IRepository<SentimentResult>
    {
        Task<IEnumerable<SentimentResult>> GetResultsByCoinIdAsync(int coinId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
