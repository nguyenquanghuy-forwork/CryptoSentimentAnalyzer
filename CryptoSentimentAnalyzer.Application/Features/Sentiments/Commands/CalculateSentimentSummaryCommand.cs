using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Features.Sentiments.Commands
{
    public class CalculateSentimentSummaryCommand : IRequest<bool>
    {
        public int CoinId { get; set; }

        public CalculateSentimentSummaryCommand(int coinId)
        {
            CoinId = coinId;
        }
    }
}
