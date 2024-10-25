using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDatabase.Data;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using MultiDatabase.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Controller
{
   public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly Application2DbContext _mockDbContext;

        public UserControllerTest()
        {
            var options = new DbContextOptionsBuilder<Application2DbContext>()
                .UseInMemoryDatabase(databaseName: "AddUserTest")
                .Options;

            _mockDbContext = new Application2DbContext(options);
            _controller = new UserController(_mockDbContext);
        }

        private async Task ClearDatabaseAsync()
        {
            _mockDbContext.Users.RemoveRange(_mockDbContext.Users);
            await _mockDbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task Add_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            await ClearDatabaseAsync(); 

            byte[] imageBytes = Encoding.UTF8.GetBytes("Test image content");
            using var stream = new MemoryStream(imageBytes);

            var mockFile = new FormFile(stream, 0, stream.Length, "File", "testimage.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var viewModel = new AddViewModel2
            {
                Name = "test name",
                Address = "test address",
                Phone = "4589636897",
                Email = "test@gmail.com",
                File = mockFile,
            };

            // Act
            var result = await _controller.Add(viewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, await _mockDbContext.Users.CountAsync());

            var addedUser = await _mockDbContext.Users.FirstOrDefaultAsync();
            Assert.NotNull(addedUser);
        }

        [Fact]
        public async Task Edit_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            await ClearDatabaseAsync(); // Clear database before running the test

            var user = new User
            {
                Id = 1,
                Name = "test name",
                Address = "test address",
                Phone = "4589636897",
                Email = "test@gmail.com",
                FileName = "oldimage.jpg"
            };
            await _mockDbContext.Users.AddAsync(user);
            await _mockDbContext.SaveChangesAsync();

            byte[] updatedImageBytes = Encoding.UTF8.GetBytes("Updated image content");
            var updatedStream = new MemoryStream(updatedImageBytes);

            var updatedMockFile = new FormFile(updatedStream, 0, updatedStream.Length, "File", "updatedimage.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var updatedUser = new User
            {
                Id = 1,
                Name = "test2 name",
                Address = "test address updated",
                Phone = "9876543210",
                Email = "updated@gmail.com",
                FileName = "updatedimage.jpg"
            };

            // Act
            var result = await _controller.Edit(updatedUser, updatedMockFile);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var updatedUserDb = await _mockDbContext.Users.FindAsync(1);
            Assert.Equal("test2 name", updatedUserDb?.Name);
            Assert.Equal("test address updated", updatedUserDb?.Address);
            Assert.Equal("9876543210", updatedUserDb?.Phone);
            Assert.Equal("updated@gmail.com", updatedUserDb?.Email);
            Assert.Equal("updatedimage.jpg", updatedUserDb?.FileName);
        }

        [Fact]
        public async Task Delete_ExistingUser_ShouldRedirectToIndex_AndDeleteFile()
        {
            // Arrange
            await ClearDatabaseAsync(); // Clear database before running the test

            var userDelete = new User
            {
                Id = 1,
                Name = "test name",
                Address = "test address",
                Phone = "4589636897",
                Email = "test@gmail.com",
                FileName = "testimage.jpg"
            };

            await _mockDbContext.Users.AddAsync(userDelete);
            await _mockDbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(userDelete.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(0, await _mockDbContext.Users.CountAsync()); // Ensure user is deleted
        }
    }
}
