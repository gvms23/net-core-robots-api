using Kodo.Robots.Domain.Interfaces.UoW;
using Kodo.Robots.Infra.Data.Context;
using System;
using System.Threading.Tasks;

namespace Kodo.Robots.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RobotsContext _context;

        public UnitOfWork(RobotsContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                int _changesNumber = await _context.SaveChangesAsync();
                return _changesNumber > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
