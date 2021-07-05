using System.Threading.Tasks;
using Cards.Infra.Context;
using Cards.Infra.Interfaces;

namespace Cards.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CardContext _context;

        public UnitOfWork(CardContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
