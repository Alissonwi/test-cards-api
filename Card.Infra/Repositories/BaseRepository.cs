using Cards.Infra.Context;

namespace Cards.Infra.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly CardContext _context;

        public BaseRepository(CardContext context)
        {
            _context = context;
        }
    }
}
