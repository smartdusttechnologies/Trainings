using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDatabase.Data;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestProject.DbContexts;
//using  TestProject.DbContexts;
namespace MultiDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmployeeDbContext _context;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            //if (builder.Services.Environment("Test"))
            //{

            //    var options = new DbContextOptionsBuilder<EmployeeDbContext>()
            //        .UseInMemoryDatabase(databaseName: "TestDb")
            //        .Options;

            //    _context = new EmployeeDbContext(options);
            //}
            //else
            //{
            //    _dbContext = dbContext;
            //}
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDb")
                    .Options;

                _context = new EmployeeDbContext(options);
            }
            else
            {
                _dbContext = dbContext;
            }
          
            //_employeeDbContext = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test"
            //     ? employeeDbContext
            //     : null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var employees = await _context.Employees.ToListAsync();
                return Ok(employees);
            }
            else
            {
                var employees = await _dbContext.Employees.ToListAsync();
                return Ok(employees);
            }
            //var employees = await _dbContext.Employees.ToListAsync();
            //return Ok(employees);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            else
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            
        }

       
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] AddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[]? fileData = null;
            string? fileName = null;

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.File.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                    fileName = viewModel.File.FileName;
                }
            }
            else
            {
                ModelState.AddModelError("File", "No valid file provided.");
                return BadRequest(ModelState);
            }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                            var employee = new Employee
                {
                    Name = viewModel.Name,
                    HomeAddress = viewModel.HomeAddress,
                    Designation = viewModel.Designation,
                    EmployeeSurname = viewModel.EmployeeSurname,
                    FileData = fileData,
                    FileName = fileName
                };

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            else
            {
                var employee = new Employee
                {
                    Name = viewModel.Name,
                    HomeAddress = viewModel.HomeAddress,
                    Designation = viewModel.Designation,
                    EmployeeSurname = viewModel.EmployeeSurname,
                    FileData = fileData,
                    FileName = fileName
                };

                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Name = viewModel.Name;
                existingEmployee.HomeAddress = viewModel.HomeAddress;
                existingEmployee.Designation = viewModel.Designation;
                existingEmployee.EmployeeSurname = viewModel.EmployeeSurname;


                if (viewModel.File != null && viewModel.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.File.CopyToAsync(memoryStream);
                        existingEmployee.FileData = memoryStream.ToArray();
                        existingEmployee.FileName = viewModel.File.FileName;
                    }
                }

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                return Ok(existingEmployee);
            }
            else
            {
                var existingEmployee = await _dbContext.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Name = viewModel.Name;
                existingEmployee.HomeAddress = viewModel.HomeAddress;
                existingEmployee.Designation = viewModel.Designation;
                existingEmployee.EmployeeSurname = viewModel.EmployeeSurname;


                if (viewModel.File != null && viewModel.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.File.CopyToAsync(memoryStream);
                        existingEmployee.FileData = memoryStream.ToArray();
                        existingEmployee.FileName = viewModel.File.FileName;
                    }
                }

                _dbContext.Employees.Update(existingEmployee);
                await _dbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }

        }

        // Delete an employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            
        }

        
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null || employee.FileData == null)
                {
                    return NotFound();
                }

                return File(employee.FileData, "application/octet-stream", employee.FileName);
            }
            else
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null || employee.FileData == null)
                {
                    return NotFound();
                }

                return File(employee.FileData, "application/octet-stream", employee.FileName);
            }
          
        }
    }
}
