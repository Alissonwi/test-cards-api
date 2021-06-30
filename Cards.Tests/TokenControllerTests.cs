using Cards.Api.Controllers;
using Cards.Infra.Context;
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
        [Fact]
        public async Task GivenTheInformationsToValidetATokenAndReturnTrue()
        {
            var mockCardsLogger = new Mock<ILogger<CardsController>>();
            var mockTokenLogger = new Mock<ILogger<TokenController>>();

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var cardsController = new CardsController(mockCardsLogger.Object, context);
            var newCostumerCard = await cardsController.CreateCostumerCard(1, "1234567891234567", "2");

            var tokenController = new TokenController(mockTokenLogger.Object, context);
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

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var cardsController = new CardsController(mockCardsLogger.Object, context);
            var newCostumerCard = await cardsController.CreateCostumerCard(1, "1234567891234567", "2");

            var tokenController = new TokenController(mockTokenLogger.Object, context);
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

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var cardsController = new CardsController(mockCardsLogger.Object, context);
            var newCostumerCard = await cardsController.CreateCostumerCard(1, "1234567891234567", "2");

            var tokenController = new TokenController(mockTokenLogger.Object, context);
            var returnOfRequest = await tokenController.ValidateToken(3, 3, 6745, "2");
            
            var okResult = returnOfRequest as ObjectResult;
            var actualResult = okResult.Value;
            Assert.IsType<ValidationProblemDetails>(actualResult);
        }

        [Theory]
        [InlineData(2, 1, 3412, "2")]
        [InlineData(1, 2, 3412, "2")]
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

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var cardsController = new CardsController(mockCardsLogger.Object, context);
            var newCostumerCard = await cardsController.CreateCostumerCard(1, "1234567891231234", "2");

            var tokenController = new TokenController(mockTokenLogger.Object, context);
            var returnOfRequest = await tokenController.ValidateToken(costumerId, cardId, token, CVV);
            var okResult = returnOfRequest as OkObjectResult;
            var actualResult = okResult.Value as Validation;

            Assert.False(actualResult.Validated);
        }
    }
}
