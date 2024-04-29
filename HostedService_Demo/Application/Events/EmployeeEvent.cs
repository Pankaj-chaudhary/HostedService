using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Events
{
    public class EmployeeEvent
    {
        public Guid EventId { get; set; }

        public EmployeeDTO Employee { get; set; }
    }
}
