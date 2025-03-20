﻿using CryptoSentimentAnalyzer.Application.DTOs;
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
    public class GetCoinBySymbolQueryHandler : IRequestHandler<GetCoinBySymbolQuery, CoinDto>
    {
        private readonly ICoinRepository _coinRepository;

        public GetCoinBySymbolQueryHandler(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<CoinDto> Handle(GetCoinBySymbolQuery request, CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetCoinBySymbolAsync(request.Symbol);

            if (coin == null)
                return null;

            return new CoinDto
            {
                Id = coin.Id,
                Symbol = coin.Symbol,
                Name = coin.Name,
                Description = coin.Description
            };
        }
    }
}
