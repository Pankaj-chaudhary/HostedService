using Application.DTO;
using Application.Events;
using MediatR;
using Service.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class CreateEmployee : IRequest<EmployeeDTO>
    {
        public EmployeeDTO Employee { get; set; }

        public CreateEmployee(EmployeeDTO employee)
        {
            Employee = employee;
        }
    }

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployee, EmployeeDTO>
    {

        private readonly IRabbitMqProducer<EmployeeEvent> _producer;

        public CreateEmployeeHandler(IRabbitMqProducer<EmployeeEvent> producer) => _producer = producer;

        public async Task<EmployeeDTO> Handle(CreateEmployee request, CancellationToken cancellationToken)
        {
            request.Employee.Id = Guid.NewGuid();
            var @event = new EmployeeEvent
            {
                EventId = Guid.NewGuid(),
                Employee = request.Employee,
            };

            _producer.Publish(@event);
            await Task.Delay(500);
            return request.Employee;
        }
    }
}
