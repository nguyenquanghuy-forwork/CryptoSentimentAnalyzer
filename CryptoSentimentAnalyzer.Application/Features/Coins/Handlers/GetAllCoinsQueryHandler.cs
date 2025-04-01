using CryptoSentimentAnalyzer.Application.Features.Coins.DTOs;
using CryptoSentimentAnalyzer.Application.Features.Coins.Queries;
using CryptoSentimentAnalyzer.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Coins.Handlers
{
    public class GetAllCoinsQueryHandler : IRequestHandler<GetAllCoinsQuery, IEnumerable<CoinDto>>
    {
        private readonly ICoinRepository _coinRepository;

        public GetAllCoinsQueryHandler(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<IEnumerable<CoinDto>> Handle(GetAllCoinsQuery request, CancellationToken cancellationToken)
        {
            var coins = await _coinRepository.GetAllAsync();

            return coins.Select(c => new CoinDto
            {
                Id = c.Id,
                Symbol = c.Symbol,
                Name = c.Name,
                Description = c.Description
            });
        }
    }
}
