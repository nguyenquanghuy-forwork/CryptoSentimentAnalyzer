using CryptoSentimentAnalyzer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Interfaces
{
    public interface ICoinRepository : IRepository<Coin>
    {
        Task<Coin> GetCoinBySymbolAsync(string symbol);
    }
}
