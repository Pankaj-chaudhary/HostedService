using Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IEmployeeService
    {
        Task<bool> CreateAsync(Employee employee);

        Task<Employee?> GetAsync(Guid id);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<bool> UpdateAsync(Employee employee);

        Task<bool> DeleteAsync(Guid id);
    }
}
