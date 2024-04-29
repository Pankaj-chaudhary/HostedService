using Persistence.Model;
using Persistence.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> CreateAsync(Employee employee)
        {
            var existingUser = await _employeeRepository.GetAsync(employee.Id);
            if (existingUser is not null)
            {
                var message = $"An employee with id {employee.Id} already exists";
                throw new ValidationException(message);
            }

            return await _employeeRepository.CreateAsync(employee);
        }

        public async Task<Employee?> GetAsync(Guid id)
        {
            return await _employeeRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
