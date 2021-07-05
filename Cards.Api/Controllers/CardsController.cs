using AutoMapper;
using Cards.Infra.Extensions;
using Cards.Infra.Interfaces;
using Cards.Infra.Models;
using Cards.Infra.Resources;
using Cards.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ILogger<CardsController> _logger;

        private readonly ICardService _cardService;

        private readonly IMapper _mapper;

        public CardsController(ILogger<CardsController> logger, ICardService cardService, IMapper mapper)
        {
            _logger = logger;
            _cardService = cardService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCostumerCard([FromBody] SaveCardResource resource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!resource.cardNumber.All(char.IsDigit) || !resource.CVV.All(char.IsDigit))
                    {
                        return ValidationProblem("Only numbers is accepted for card number and CVV");
                    }

                    var card = _mapper.Map<SaveCardResource, Card>(resource);

                    card.TokenRegistrationDate = DateTime.Now;

                    var token = TokenGenerator.GetTokenFromCardNumber(resource.cardNumber, int.Parse(resource.CVV));

                    var result = await _cardService.SaveAsync(card);

                    if (!result.Success)
                        return BadRequest(result.Message);

                    return Ok(new
                    {
                        RegistrationDate = card.TokenRegistrationDate,
                        Token = token,
                        CardId = card.CardId
                    });
                }
                return BadRequest(ModelState.GetErrorMessages());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ValidationProblem(e.Message);
            }
        }
    }
}
