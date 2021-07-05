using System;
using System.Threading.Tasks;
using Cards.Infra.Interfaces;
using Cards.Infra.Models;
using Cards.Infra.Services.Communication;

namespace Cards.Infra.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(ICardRepository cardRepository, IUnitOfWork unitOfWork)
        {
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Card> GetCardByCardId(int cardId)
        {
            return await _cardRepository.GetCardByCardId(cardId);
        }

        public async Task<SaveCardResponse> SaveAsync(Card card)
        {
            try
            {
                await _cardRepository.AddAsync(card);
                await _unitOfWork.CompleteAsync();

                return new SaveCardResponse(card);
            }
            catch (Exception ex)
            {
                return new SaveCardResponse($"An error occurred when saving the card: {ex.Message}");
            }
        }
    }
}
