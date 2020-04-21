using System;
using Kodo.Robots.Domain.Core.Events;

namespace Kodo.Robots.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();
    }
}