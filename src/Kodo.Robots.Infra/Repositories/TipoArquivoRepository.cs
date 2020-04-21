using Kodo.Robots.Domain.Entities;
using Kodo.Robots.Domain.Interfaces.Repositories;
using Kodo.Robots.Infra.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kodo.Robots.Infra.Data.Repositories
{
    public class RobotRepository : Repository<Robot>, IRobotRepository
    {
        public RobotRepository(RobotsContext context)
            : base(context)
        {
        }

        public Task<Robot> GetByName(string name)
            => FindAsync(wh => !wh.IsDeleted 
                                && string.Equals(wh.Name, name, StringComparison.CurrentCultureIgnoreCase));

        public bool RobotExists(string name)
            => Query(wh => !wh.IsDeleted
                            && string.Equals(wh.Name, name, StringComparison.CurrentCultureIgnoreCase)).Any();
    }
}
