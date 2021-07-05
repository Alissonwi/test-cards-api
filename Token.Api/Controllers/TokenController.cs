using Cards.Infra.Interfaces;
using Cards.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Token.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly ICardService _cardService;

        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger, ICardService cardService)
        {
            _logger = logger;
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateToken(int costumerId, int cardId, long token, string CVV)
        {
            try
            {

                if (costumerId == 0 || cardId == 0 || token == 0 || string.IsNullOrEmpty(CVV))
                {
                    return ValidationProblem("All the attributes need to have a value");
                }

                var card = await _cardService.GetCardByCardId(cardId);

                if (card == null)
                {
                    return ValidationProblem("The cardId don't exists.");
                }

                var tokenCard = TokenGenerator.GetTokenFromCardNumber(card.CardNumber.ToString(), int.Parse(CVV));

                if (card.CostumerId != costumerId || DateTime.Now > card.TokenRegistrationDate.AddMinutes(30) || tokenCard != token)
                {
                    return Ok(new Validation { Validated = false });
                }

                return Ok(new Validation { Validated = true });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ValidationProblem(e.Message);
            }
        }
    }
}
