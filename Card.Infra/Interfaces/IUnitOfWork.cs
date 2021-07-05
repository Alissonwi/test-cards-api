using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Infra.Interfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
