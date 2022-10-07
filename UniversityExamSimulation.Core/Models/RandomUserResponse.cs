using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamSimulation.Core.Models
{
    public class RandomUserResponse
    {
        [JsonProperty("results")]
        public RandomUser[] RandomUsers { get; set; }
    }
    public class RandomUser
    {
        public Name Name { get; set; }
    }

    public class Name
    {
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
    }
}
