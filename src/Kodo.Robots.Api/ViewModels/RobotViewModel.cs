using System;

namespace Kodo.Robots.Api.ViewModels
{
    public class RobotViewModel : HateoasViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }
    }
}
