using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persistence.Model
{
    public class Employee
    {
        public Guid Id { get; set; }
        [JsonPropertyName("PK")]
        public string Pk => Id.ToString();
        [JsonPropertyName("SK")]
        public string SK => Id.ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
