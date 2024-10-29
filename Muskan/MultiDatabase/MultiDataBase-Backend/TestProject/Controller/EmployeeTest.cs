using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiDatabase.Controllers;
using MultiDatabase.Models.Entities;
using MultiDatabase.Models;
using MultiDatabase.Repository.Interface;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.Controller
{
    public class EmployeeTest
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;

        public EmployeeTest()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _employeeController = new EmployeeController(_mockEmployeeRepository.Object, null);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnOkResult_WithEmployeeList()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", HomeAddress = "123 Street", Designation = "Developer", EmployeeSurname = "Doe" }
            };
            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployeeAsync()).ReturnsAsync(employees);

            // Act
            var result = await _employeeController.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task GetEmployee_ExistingId_ShouldReturnOkResult_WithEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Jane Doe", HomeAddress = "456 Avenue", Designation = "Manager", EmployeeSurname = "Doe" };
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(1)).ReturnsAsync(employee);

            // Act
            var result = await _employeeController.GetEmployee(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnedEmployee.Id);
        }

        [Fact]
        public async Task GetEmployee_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(999)).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeController.GetEmployee(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ValidEmployee_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("file content")), 0, 12, "Data", "testfile.txt");
            var viewModel = new AddViewModel
            {
                Name = "New Employee",
                HomeAddress = "789 Maple St",
                Designation = "Intern",
                EmployeeSurname = "EmployeeSurname",
                File = fileMock
            };

            var createdEmployee = new Employee
            {
                Id = 1,
                Name = viewModel.Name,
                HomeAddress = viewModel.HomeAddress,
                Designation = viewModel.Designation,
                EmployeeSurname = viewModel.EmployeeSurname,
                FileName = viewModel.File.FileName
            };

          
            _mockEmployeeRepository.Setup(repo => repo.AddEmplyooAsync(It.IsAny<Employee>()))
                              .Returns(Task.CompletedTask);


            // Act
            var result = await _employeeController.Post(viewModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var employee = Assert.IsType<Employee>(createdAtActionResult.Value);
            Assert.Equal(viewModel.Name, employee.Name);
        }


        [Fact]
        public async Task Put_ExistingEmployee_ShouldReturnOkResult_WithUpdatedEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Old Name", HomeAddress = "Old Address", Designation = "Old Designation", EmployeeSurname = "Old EmployeeSurname" };
            var updatedViewModel = new AddViewModel
            {
                Name = "Updated Name",
                HomeAddress = "Updated Address",
                Designation = "Updated Designation",
                EmployeeSurname = "Updated EmployeeSurname"
            };
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(1)).ReturnsAsync(employee);
            _mockEmployeeRepository.Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeController.Put(1, updatedViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedEmployee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal("Updated Name", updatedEmployee.Name);
        }

        [Fact]
        public async Task Delete_ExistingEmployee_ShouldReturnNoContent()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "ToBeDeleted" };
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(1)).ReturnsAsync(employee);
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeController.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DownloadFile_EmployeeExists_ShouldReturnFile()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "Employee with File", FileData = Encoding.UTF8.GetBytes("file content"), FileName = "file.txt" };
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(1)).ReturnsAsync(employee);

            // Act
            var result = await _employeeController.DownloadFile(1);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal(employee.FileData, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadFile_EmployeeDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(999)).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeController.DownloadFile(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
