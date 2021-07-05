using System.Threading.Tasks;
using Cards.Infra.Models;

namespace Cards.Infra.Interfaces
{
    public interface ICardRepository
    {
        Task<Card> GetCardByCardId(int cardId);

        Task AddAsync(Card card);
    }
}
