using System;

namespace Kodo.Robots.Domain.Core.Events
{
    public abstract class Event : Message, INotification
    {
        public DateTime CreatedDate { get; private set; }

        protected Event()
        {
            CreatedDate = DateTime.Now;
        }
    }
}