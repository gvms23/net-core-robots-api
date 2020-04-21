using System;
using System.Threading.Tasks;

namespace Kodo.Robots.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// SaveChanges in EFCore context.
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitAsync();
    }
}
