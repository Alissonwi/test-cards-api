using AutoMapper;
using Cards.Api.Controllers;
using Cards.Infra.Context;
using Cards.Infra.Interfaces;
using Cards.Infra.Mapping;
using Cards.Infra.Repositories;
using Cards.Infra.Resources;
using Cards.Infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cards.Tests
{
    public class CardsControllerTests
    {
        private static IMapper _mapper;

        private static ICardService _cardService;

        private static ICardRepository _cardRepository;

        private static IUnitOfWork _unitOfWork;

        public CardsControllerTests()
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
        public async Task GivenTheInformationsToGenerateANewCardAndReturn200()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<CardsController>>();
            var controller = new CardsController(mockLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456791234567";
            resource.CVV = "1234";

            //Act
            var returnOfRequest = await controller.CreateCostumerCard(resource);

            //Assert
            Assert.IsType<OkObjectResult>(returnOfRequest); //200
        }

        [Fact]
        public async Task GivenTheInformationsToGenerateANewCardWithACostumerIdAndCardNumberThatAlreadyExistsAndReturn400()
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var controller = new CardsController(mockLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = "123456789123456";
            resource.CVV = "1234";

            var newCostumerCard = await controller.CreateCostumerCard(resource);

            var returnOfRequest = await controller.CreateCostumerCard(resource);

            Assert.IsType<BadRequestObjectResult>(returnOfRequest);
        }

        [Fact]
        public async Task GivenTheInformationsToGenerateANewCardWithCostumerIdZeroAndReturn400()
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var controller = new CardsController(mockLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 0;
            resource.cardNumber = "1234567891234567";
            resource.CVV = "1234";

            var returnOfRequest = await controller.CreateCostumerCard(resource);

            Assert.IsType<ObjectResult>(returnOfRequest);
        }

        [Theory]
        [InlineData("123456789123467", "")]
        [InlineData("", "1234")]
        [InlineData("", "")]
        public async Task GivenTheInformationsToGenerateANewCardWithCardNumberOrCVVWithNoValueAndReturn400(
            string cardNumber,
            string CVV)
        {
            var mockLogger = new Mock<ILogger<CardsController>>();

            var controller = new CardsController(mockLogger.Object, _cardService, _mapper);
            var resource = new SaveCardResource();
            resource.costumerId = 1;
            resource.cardNumber = cardNumber;
            resource.CVV = CVV;

            var returnOfRequest = await controller.CreateCostumerCard(resource);

            Assert.IsType<ObjectResult>(returnOfRequest);
        }
    }
}
