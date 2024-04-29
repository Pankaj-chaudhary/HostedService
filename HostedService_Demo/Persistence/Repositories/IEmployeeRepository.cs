using Persistence.Model;

namespace Persistence.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> CreateAsync(Employee employee);

        Task<Employee?> GetAsync(Guid id);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<bool> UpdateAsync(Employee employee);

        Task<bool> DeleteAsync(Guid id);
    }
}
