using MultiDatabase.Models.Entities;

namespace MultiDatabase.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee> GetEmployeeById(int id);
        Task AddEmplyooAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployee(int id);
    }
}
