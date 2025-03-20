using CryptoSentimentAnalyzer.Application.DTOs;
using CryptoSentimentAnalyzer.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSentimentAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoinsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinDto>>> GetAllCoins()
        {
            var result = await _mediator.Send(new GetAllCoinsQuery());
            return Ok(result);
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<CoinDto>> GetCoinBySymbol(string symbol)
        {
            var result = await _mediator.Send(new GetCoinBySymbolQuery(symbol));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
