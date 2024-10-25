using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDatabase.Controllers;
using MultiDatabase.Data;
using MultiDatabase.Models.Entities;
using MultiDatabase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Text;

namespace TestProject.Controller
{
    public class EmployeeTest : IDisposable
    {
        private readonly EmployeeController _employeeController;
        private readonly ApplicationDbContext _employeeContext;

        public EmployeeTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDbEmployee")
                 .Options;
            _employeeContext = new ApplicationDbContext(options);
            _employeeController = new EmployeeController(_employeeContext);
        }

        public void Dispose()
        {
            _employeeContext.Database.EnsureDeleted();
            _employeeContext.Dispose();
        }

        [Fact]
        public async Task GetEmployee_List()
        {
            // Arrange 
            _employeeContext.Employees.Add(new Employee
            {
                Name = "test name",
                HomeAddress = "Test address",
                Designation = "test",
                EmployeeSurname = "test EmployeeSurname"
            });
            await _employeeContext.SaveChangesAsync();

            // Act 
            var result = await _employeeController.GetEmployees();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<Employee>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);

            Assert.Single(employees);
        }

        [Fact]
        public async Task Post_IsValidWithFile()
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

            // Act
            var result = await _employeeController.Post(viewModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var employee = Assert.IsType<Employee>(createdAtActionResult.Value);
            Assert.Equal(viewModel.Name, employee.Name);
            Assert.Equal("testfile.txt", employee.FileName);
            Assert.Equal(1, await _employeeContext.Employees.CountAsync());
        }

        [Fact]

        public async Task Put_Employee()
        {
            // Arrange
            var viewModelToUpdate = new AddViewModel
            {
                Name = "Updated Name",
                HomeAddress = "Updated Address",
                Designation = "Updated Designation",
                EmployeeSurname = "Updated EmployeeSurname"
            };

            // Act
            var result = await _employeeController.Put(999, viewModelToUpdate);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Put_WhenEmployeeIsUpdatedWithFile()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "Old Name",
                HomeAddress = "Old Address",
                Designation = "Old Designation",
                EmployeeSurname = "Old EmployeeSurname"
            };
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            var fileMock = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("updated file content")), 0, 20, "Data", "updatedfile.txt");
            var viewModel = new AddViewModel
            {
                Name = "Updated Name",
                HomeAddress = "Updated Address",
                Designation = "Updated Designation",
               EmployeeSurname = "Updated EmployeeSurname",
                File = fileMock
            };

            // Act
            var result = await _employeeController.Put(employee.Id, viewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployee = Assert.IsType<Employee>(okResult.Value);

            Assert.Equal("Updated Name", returnedEmployee.Name);
            Assert.Equal("Updated Address", returnedEmployee.HomeAddress);
            Assert.Equal("Updated Designation", returnedEmployee.Designation);
            Assert.Equal("Updated EmployeeSurname", returnedEmployee.EmployeeSurname);
            Assert.Equal("updatedfile.txt", returnedEmployee.FileName);
            Assert.Equal(1, await _employeeContext.Employees.CountAsync());
        }



        [Fact]
        public async Task Delete_WhenEmployeeDoesNotExist()
        {
            // Act
            var result = await _employeeController.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_EmployeeIsDeleted()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "ToBeDeleted",
                HomeAddress = "ToBeDeleted Address",
                Designation = "ToBeDeleted Designation",
                EmployeeSurname = "ToDeleted EmployeeSurname"
            };
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            // Act
            var result = await _employeeController.Delete(employee.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Null(await _employeeContext.Employees.FindAsync(employee.Id));
        }

        [Fact]
        public async Task DownloadFile_FileExists()
        {
            // Arrange
            var fileContent = "file content";
            var employee = new Employee
            {
                Name = "Employee with File",
                HomeAddress = "File Address",
                Designation = "File Designation",
                EmployeeSurname = "File Designation",
                FileData = Encoding.UTF8.GetBytes(fileContent),
                FileName = "file.txt"
            };
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();

            // Act
            var result = await _employeeController.DownloadFile(employee.Id);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal(employee.FileData, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadFile_FileDoesNotExist()
        {
            // Act
            var result = await _employeeController.DownloadFile(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
