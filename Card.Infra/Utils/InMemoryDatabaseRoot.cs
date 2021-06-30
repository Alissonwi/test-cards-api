using Microsoft.EntityFrameworkCore.Storage;

namespace Cards.Infra.Utils
{
    public class InMemoryDatabaseRootClass
    {
        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();
    }
}
