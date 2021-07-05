using System.Threading.Tasks;
using Cards.Infra.Models;
using Cards.Infra.Services.Communication;

namespace Cards.Infra.Interfaces
{
    public interface ICardService
    {
        Task<Card> GetCardByCardId(int cardId);

        Task<SaveCardResponse> SaveAsync(Card card);
    }
}
