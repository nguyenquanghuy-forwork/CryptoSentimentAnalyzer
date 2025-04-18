﻿using CryptoSentimentAnalyzer.Application.Features.Sentiments.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Sentiments.Queries
{
    public class GetSentimentSummaryByCoinQuery : IRequest<SentimentSummaryDto>
    {
        public string CoinSymbol { get; set; }

        public GetSentimentSummaryByCoinQuery(string coinSymbol)
        {
            CoinSymbol = coinSymbol;
        }
    }
}
