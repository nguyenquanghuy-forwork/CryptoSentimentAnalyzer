using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace CryptoSentimentAnalyzer.API.Hubs
{
    public class SentimentHub : Hub
    {
        public async Task JoinCoinGroup(string coinSymbol)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, coinSymbol);
        }

        public async Task LeaveCoinGroup(string coinSymbol)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, coinSymbol);
        }
    }
}
