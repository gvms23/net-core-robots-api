using Kodo.Robots.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Kodo.Robots.Domain.Interfaces.Repositories
{
    public interface IRobotRepository : IRepository<Robot>
    {
        Task<Robot> GetByName(string name);
        bool RobotExists(string name);
    }
}
