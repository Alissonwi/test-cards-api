using AutoMapper;
using Cards.Api.Controllers;
using Cards.Infra.Context;
using Cards.Infra.Interfaces;
using Cards.Infra.Mapping;
using Cards.Infra.Repositories;
using Cards.Infra.Resources;
using Cards.Infra.Services;
using Cards.Infra.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Token.Api.Controllers;
using Xunit;

namespace Cards.Tests
{
    public class TokenControllerTests
    {
        private static IMapper _mapper;

        private static ICardService _cardService;

        private static ICardRepository _cardRepository;

        private static IUnitOfWork _unitOfWork;

        public TokenControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ResourceToModelProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            if (_cardRepository == null)
            {
                ICardRepository cardRepository = new CardRepository(context);
                _cardRepository = cardRepository;
            }

            if (_unitOfWork == null)
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                _unitOfWork = unitOfWork;
            }

            if (_cardService == null)
            {
                ICardService cardService = new CardService(_cardRepository, _unitOfWork);
                _cardService = cardService;
            }
        }

        [Fact]
        public async Task GivenTheInformationsToValidetATokenAndReturnTrue()
        {
            var mockCardsLogger = new Mock<ILogger<CardsController>>();
            var mockTokenLogger = new Mock<ILogger<TokenController>>();

            var cardsController = new CardsController(mockCardsLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456791234567";
            resource.CVV = "2";
            var newCostumerCard = await cardsController.CreateCostumerCard(resource);

            var tokenController = new TokenController(mockTokenLogger.Object, _cardService);
            var returnOfRequest = await tokenController.ValidateToken(1, 1, 6745, "2");
            var okResult = returnOfRequest as OkObjectResult;
            var actualResult = okResult.Value as Validation;

            Assert.True(actualResult.Validated);
        }

        [Theory]
        [InlineData(0, 1, 1234, "1324")]
        [InlineData(1, 0, 1234, "1324")]
        [InlineData(1, 1, 0, "1324")]
        [InlineData(1, 1, 1234, "")]
        public async Task GivenTheInformationsToValidetATokenWithoutOneOfTheValuesAndReturnError400(
            int costumerId,
            int cardId,
            long token,
            string CVV)
        {
            var mockCardsLogger = new Mock<ILogger<CardsController>>();
            var mockTokenLogger = new Mock<ILogger<TokenController>>();

            var cardsController = new CardsController(mockCardsLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456791234567";
            resource.CVV = "2";
            var newCostumerCard = await cardsController.CreateCostumerCard(resource);

            var tokenController = new TokenController(mockTokenLogger.Object, _cardService);
            var returnOfRequest = await tokenController.ValidateToken(costumerId, cardId, token, CVV);

            var okResult = returnOfRequest as ObjectResult;
            var actualResult = okResult.Value;
            Assert.IsType<ValidationProblemDetails>(actualResult);
        }

        [Fact]
        public async Task GivenTheInformationsToValidetATokenWithACostumerIdThatDontExistAndReturnError400()
        {
            var mockCardsLogger = new Mock<ILogger<CardsController>>();
            var mockTokenLogger = new Mock<ILogger<TokenController>>();

            var cardsController = new CardsController(mockCardsLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456791234567";
            resource.CVV = "2";
            var newCostumerCard = await cardsController.CreateCostumerCard(resource);

            var tokenController = new TokenController(mockTokenLogger.Object, _cardService);
            var returnOfRequest = await tokenController.ValidateToken(10, 10, 6745, "2");
            
            var okResult = returnOfRequest as ObjectResult;
            var actualResult = okResult.Value;
            Assert.IsType<ValidationProblemDetails>(actualResult);
        }

        [Theory]
        [InlineData(20, 1, 3412, "2")]
        [InlineData(1, 1, 3312, "2")]
        [InlineData(1, 1, 3412, "3")]
        public async Task GivenTheWrongInformationsToValidetATokenAndReturnFalse(
            int costumerId,
            int cardId,
            long token,
            string CVV)
        {
            var mockCardsLogger = new Mock<ILogger<CardsController>>();
            var mockTokenLogger = new Mock<ILogger<TokenController>>();

            var cardsController = new CardsController(mockCardsLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456791234567";
            resource.CVV = "2";
            var newCostumerCard = await cardsController.CreateCostumerCard(resource);

            var tokenController = new TokenController(mockTokenLogger.Object, _cardService);
            var returnOfRequest = await tokenController.ValidateToken(costumerId, cardId, token, CVV);
            var okResult = returnOfRequest as OkObjectResult;
            var actualResult = okResult.Value as Validation;

            Assert.False(actualResult.Validated);
        }
    }
}
