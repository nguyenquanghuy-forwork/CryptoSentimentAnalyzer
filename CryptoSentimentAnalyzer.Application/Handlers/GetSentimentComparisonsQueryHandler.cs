using CryptoSentimentAnalyzer.Application.DTOs;
using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Application.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Handlers
{
    public class GetSentimentComparisonsQueryHandler : IRequestHandler<GetSentimentComparisonsQuery, IEnumerable<SentimentSummaryDto>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ISentimentSummaryRepository _summaryRepository;

        public GetSentimentComparisonsQueryHandler(
            ICoinRepository coinRepository,
            ISentimentSummaryRepository summaryRepository)
        {
            _coinRepository = coinRepository;
            _summaryRepository = summaryRepository;
        }

        public async Task<IEnumerable<SentimentSummaryDto>> Handle(GetSentimentComparisonsQuery request, CancellationToken cancellationToken)
        {
            var result = new List<SentimentSummaryDto>();
            var coinIds = new List<int>();
            var coinMap = new Dictionary<int, string>();
            var nameMap = new Dictionary<int, string>();

            foreach (var symbol in request.CoinSymbols)
            {
                var coin = await _coinRepository.GetCoinBySymbolAsync(symbol);
                if (coin != null)
                {
                    coinIds.Add(coin.Id);
                    coinMap[coin.Id] = coin.Symbol;
                    nameMap[coin.Id] = coin.Name;
                }
            }

            var summaries = await _summaryRepository.GetSummariesByCoinIdsAsync(coinIds);

            foreach (var summary in summaries)
            {
                result.Add(new SentimentSummaryDto
                {
                    CoinId = summary.CoinId,
                    CoinSymbol = coinMap[summary.CoinId],
                    CoinName = nameMap[summary.CoinId],
                    PositivePercentage = summary.PositivePercentage,
                    NegativePercentage = summary.NegativePercentage,
                    NeutralPercentage = summary.NeutralPercentage,
                    TotalTweets = summary.TotalTweets,
                    CalculatedAt = summary.CalculatedAt
                });
            }

            return result;
        }
    }
}
