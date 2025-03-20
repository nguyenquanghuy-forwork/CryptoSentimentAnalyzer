using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSentimentAnalyzer.Application.Commands
{
    public class AnalyzeCoinSentimentCommand : IRequest<bool>
    {
        public string CoinSymbol { get; set; }
        public int TweetCount { get; set; } = 100;

        public AnalyzeCoinSentimentCommand(string coinSymbol, int tweetCount = 100)
        {
            CoinSymbol = coinSymbol;
            TweetCount = tweetCount;
        }
    }
}
