using CryptoSentimentAnalyzer.Application.Features.Sentiments.DTOs;
using CryptoSentimentAnalyzer.Application.Features.Sentiments.Queries;
using CryptoSentimentAnalyzer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Sentiments.Handlers
{
    public class GetSentimentSummaryByCoinQueryHandler : IRequestHandler<GetSentimentSummaryByCoinQuery, SentimentSummaryDto>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ISentimentSummaryRepository _summaryRepository;

        public GetSentimentSummaryByCoinQueryHandler(
            ICoinRepository coinRepository,
            ISentimentSummaryRepository summaryRepository)
        {
            _coinRepository = coinRepository;
            _summaryRepository = summaryRepository;
        }

        public async Task<SentimentSummaryDto> Handle(GetSentimentSummaryByCoinQuery request, CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetCoinBySymbolAsync(request.CoinSymbol);
            if (coin == null)
                return null;

            var summary = await _summaryRepository.GetLatestSummaryByCoinIdAsync(coin.Id);
            if (summary == null)
                return new SentimentSummaryDto
                {
                    CoinId = coin.Id,
                    CoinSymbol = coin.Symbol,
                    CoinName = coin.Name,
                    PositivePercentage = 0,
                    NegativePercentage = 0,
                    NeutralPercentage = 0,
                    TotalTweets = 0,
                    CalculatedAt = DateTime.UtcNow
                };

            return new SentimentSummaryDto
            {
                CoinId = summary.CoinId,
                CoinSymbol = coin.Symbol,
                CoinName = coin.Name,
                PositivePercentage = summary.PositivePercentage,
                NegativePercentage = summary.NegativePercentage,
                NeutralPercentage = summary.NeutralPercentage,
                TotalTweets = summary.TotalTweets,
                CalculatedAt = summary.CalculatedAt
            };
        }
    }
}
