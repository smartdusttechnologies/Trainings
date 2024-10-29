using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiDatabase.Data;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using MultiDatabase.Repository.Interface;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MultiDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly  ApplicationDbContext _employeeContext;

        public EmployeeController(IEmployeeRepository employeeRepository, ApplicationDbContext employeeContext)
        {
            _employeeRepository = employeeRepository;
           //_employeeContext = employeeContext;
        }

      

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeeAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
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

            var employee = new Employee
            {
                Name = viewModel.Name,
                HomeAddress = viewModel.HomeAddress,
                Designation = viewModel.Designation,
                EmployeeSurname = viewModel.EmployeeSurname,
                FileData = fileData,
                FileName = fileName
            };

            await _employeeRepository.AddEmplyooAsync(employee); 
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetEmployeeById(id);
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

            await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
            return Ok(existingEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployee(id);
            return NoContent();
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null || employee.FileData == null)
            {
                return NotFound();
            }

            return File(employee.FileData, "application/octet-stream", employee.FileName);
        }
    }
}
