using System.Threading.Tasks;
using Kodo.Robots.Domain.Core.Commands;
using Kodo.Robots.Domain.Core.Events;

namespace Kodo.Robots.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
