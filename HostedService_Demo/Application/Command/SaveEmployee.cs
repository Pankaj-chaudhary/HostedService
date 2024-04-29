using Application.DTO;
using Application.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class SaveEmployee : IRequest<Unit>
    {
        public Guid EventId { get; set; }

        public EmployeeDTO Employee { get; set; }
    }

    public class SaveEmployeeHandler : IRequestHandler<SaveEmployee, Unit>
    {
        private readonly ILogger<SaveEmployeeHandler> _logger;
        private readonly IEmployeeService _employeeService;

        public SaveEmployeeHandler(ILogger<SaveEmployeeHandler> logger, IEmployeeService employeeService)
        {
            _logger = logger; 
            _employeeService = employeeService;
        }

        public async Task<Unit> Handle(SaveEmployee request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("---- Received message: {Message} ----", request.Employee);
            var employee = request.Employee.ToDomain();
            try
            {
                var response = await _employeeService.CreateAsync(employee);
            }
            catch (Exception ex) {
                _logger.LogError($"Error occurred: ${ex.Message}");
            }
            return await Task.FromResult(Unit.Value);
        }
    }
}
