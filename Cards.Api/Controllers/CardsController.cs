using Cards.Infra.Context;
using Cards.Infra.Entities;
using Cards.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly CardContext _context;

        private readonly ILogger<CardsController> _logger;

        public CardsController(ILogger<CardsController> logger, CardContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCostumerCard(int costumerId, string cardNumber, string CVV)
        {
            try
            {
                if (costumerId == 0 || string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(CVV))
                {
                    return ValidationProblem("All the attributes need to have a value");
                }
            
                if (cardNumber.Length > 16 || cardNumber.Length < 4)
                {
                    return ValidationProblem("The number of characters for card number has to be between 4 and 16");
                }

                if (CVV.Length > 5)
                {
                    return ValidationProblem("The number of characters for CVV has to be 5 or less");
                }

                if (!cardNumber.All(char.IsDigit) || !CVV.All(char.IsDigit))
                {
                    return ValidationProblem("Only numbers is accepted for card number and CVV");
                }

                var token = TokenGenerator.GetTokenFromCardNumber(cardNumber, int.Parse(CVV));

                var card = new Card
                {
                    CardNumber = long.Parse(cardNumber),
                    CostumerId = costumerId,
                    TokenRegistrationDate = DateTime.Now
                };

                _context.Cards.Add(card);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    RegistrationDate = card.TokenRegistrationDate,
                    Token = token,
                    CardId = card.CardId
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ValidationProblem(e.Message);
            }
        }
    }
}
