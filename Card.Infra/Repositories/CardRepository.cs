using System.Linq;
using System.Threading.Tasks;
using Cards.Infra.Context;
using Cards.Infra.Interfaces;
using Cards.Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards.Infra.Repositories
{
    public class CardRepository : BaseRepository, ICardRepository
    {
        public CardRepository(CardContext context) : base(context)
        {
        }

        public async Task AddAsync(Card card)
        {
            await _context.Cards.AddAsync(card);
        }

        public async Task<Card> GetCardByCardId(int cardId)
        {
            return await _context.Cards.Where(x => x.CardId == cardId).FirstOrDefaultAsync();
        }
    }
}
