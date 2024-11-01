using Microsoft.EntityFrameworkCore;
using MultiDatabase.Data;
using MultiDatabase.Models.Entities;
using MultiDatabase.Repository.Interface;

namespace MultiDatabase.Repository
{
    public class EmployeeRepository :IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public EmployeeRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddEmplyooAsync(Employee employee)
        {
            await _dbcontext.Employees.AddAsync(employee);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            return await _dbcontext.Employees.ToListAsync();
        }
        public async Task<Employee> GetEmployeeById(int id)
        {                     
            return await _dbcontext.Employees.FindAsync(id);
        }
        public async Task DeleteEmployee(int id)
        {
            var employee = await GetEmployeeById(id);
            if (employee != null)
            {
                _dbcontext.Employees.Remove(employee);
                await _dbcontext.SaveChangesAsync();
            }
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbcontext.Employees.Update(employee);
            await _dbcontext.SaveChangesAsync();
        }

       
    }
    }

