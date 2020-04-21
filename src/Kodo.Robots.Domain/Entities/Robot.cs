using Newtonsoft.Json.Linq;
using System;

namespace Kodo.Robots.Domain.Entities
{
    public class Robot : EntityBase
    {
        public Robot()
        {
            
        }
        public Robot(string data)
        {
            AssignDataAndName(data);
        }

        public string Data { get; private set; }
        public string Name { get; private set; }

        public void UpdateData(string data)
        {
            Data = data;
            ModifiedDate = DateTime.Now;
        }

        private void AssignDataAndName(string data)
        {
            Data = data;

            JObject _data = JObject.Parse(Data);
            if (_data.TryGetValue(nameof(Name), out JToken value))
                Name = value.ToString();
        }
    }
}
