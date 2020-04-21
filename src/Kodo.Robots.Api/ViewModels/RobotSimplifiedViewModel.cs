using System;
using Newtonsoft.Json;

namespace Kodo.Robots.Api.ViewModels
{
    public class RobotSimplifiedViewModel
    {
        public string Name { get; set; }
        
        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
    }
}
