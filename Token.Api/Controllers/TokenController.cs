using Cards.Infra.Context;
using Cards.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Token.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly CardContext _context;

        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger, CardContext context)
        {
            _logger = logger;
            _context = context;
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

                var card = _context.Cards
                    .Where(x => x.CardId == cardId)
                    .FirstOrDefault();

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
