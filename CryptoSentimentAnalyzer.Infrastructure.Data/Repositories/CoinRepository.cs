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
    public class CoinRepository : Repository<Coin>, ICoinRepository
    {
        public CoinRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Coin> GetCoinBySymbolAsync(string symbol)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Symbol.ToLower() == symbol.ToLower());
        }
    }
}
