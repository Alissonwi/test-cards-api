using Cards.Api.Controllers;
using Cards.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cards.Tests
{
    public class CardsControllerTests
    {
        [Fact]
        public void GivenTheInformationsToGenerateANewCardAndReturn200()
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var controller = new CardsController(mockLogger.Object, context);

            var returnOfRequest = controller.CreateCostumerCard(1, "1234567891234567", "1234");

            Assert.IsType<OkObjectResult>(returnOfRequest.Result); //200
        }

        [Fact]
        public void GivenTheInformationsToGenerateANewCardWithACostumerIdAndCardNumberThatAlreadyExistsAndReturn400()
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var controller = new CardsController(mockLogger.Object, context);

            var newCostumerCard = controller.CreateCostumerCard(1, "1234567891234567", "1234");

            var returnOfRequest = controller.CreateCostumerCard(1, "1234567891234567", "1234");

            Assert.IsType<ObjectResult>(returnOfRequest.Result);
        }

        [Fact]
        public void GivenTheInformationsToGenerateANewCardWithCostumerIdZeroAndReturn400()
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var controller = new CardsController(mockLogger.Object, context);

            var returnOfRequest = controller.CreateCostumerCard(0, "1234567891234567", "1234");

            Assert.IsType<ObjectResult>(returnOfRequest.Result);
        }

        [Theory]
        [InlineData("1234567891234567", "")]
        [InlineData("", "1234")]
        [InlineData("", "")]
        public void GivenTheInformationsToGenerateANewCardWithCardNumberOrCVVWithNoValueAndReturn400(
            string cardNumber,
            string CVV)
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var options = new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase("DbCardsContext")
                .Options;
            var context = new CardContext(options);

            var controller = new CardsController(mockLogger.Object, context);

            var returnOfRequest = controller.CreateCostumerCard(1, cardNumber, CVV);

            Assert.IsType<ObjectResult>(returnOfRequest.Result);
        }
    }
}
