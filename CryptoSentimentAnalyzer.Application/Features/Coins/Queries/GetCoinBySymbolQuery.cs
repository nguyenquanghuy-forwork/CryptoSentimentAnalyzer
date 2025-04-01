using CryptoSentimentAnalyzer.Application.Features.Coins.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Coins.Queries
{
    public class GetCoinBySymbolQuery : IRequest<CoinDto>
    {
        public string Symbol { get; set; }

        public GetCoinBySymbolQuery(string symbol)
        {
            Symbol = symbol;
        }
    }
}
