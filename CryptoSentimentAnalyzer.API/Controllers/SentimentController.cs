using CryptoSentimentAnalyzer.Application.Features.Coins.Commands;
using CryptoSentimentAnalyzer.Application.Features.Sentiments.DTOs;
using CryptoSentimentAnalyzer.Application.Features.Sentiments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSentimentAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SentimentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("coin/{symbol}")]
        public async Task<ActionResult<SentimentSummaryDto>> GetSentimentByCoin(string symbol)
        {
            var result = await _mediator.Send(new GetSentimentSummaryByCoinQuery(symbol));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("analyze/{symbol}")]
        public async Task<IActionResult> AnalyzeCoinSentiment(string symbol, [FromQuery] int tweetCount = 100)
        {
            var result = await _mediator.Send(new AnalyzeCoinSentimentCommand(symbol, tweetCount));

            if (!result)
                return BadRequest("Failed to analyze sentiment for the coin.");

            return Ok("Sentiment analysis started for the coin.");
        }

        [HttpPost("compare")]
        public async Task<ActionResult<IEnumerable<SentimentSummaryDto>>> CompareSentiments([FromBody] List<string> symbols)
        {
            if (symbols == null || !symbols.Any())
                return BadRequest("No coin symbols provided for comparison.");

            var result = await _mediator.Send(new GetSentimentComparisonsQuery(symbols));

            return Ok(result);
        }
    }
}
