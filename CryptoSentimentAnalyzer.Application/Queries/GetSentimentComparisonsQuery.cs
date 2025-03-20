using CryptoSentimentAnalyzer.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Queries
{
    public class GetSentimentComparisonsQuery : IRequest<IEnumerable<SentimentSummaryDto>>
    {
        public List<string> CoinSymbols { get; set; }

        public GetSentimentComparisonsQuery(List<string> coinSymbols)
        {
            CoinSymbols = coinSymbols;
        }
    }
}
