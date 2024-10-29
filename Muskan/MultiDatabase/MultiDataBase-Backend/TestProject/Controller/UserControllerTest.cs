using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiDatabase.Controllers;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using MultiDatabase.Repository.Interface;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserControllerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _controller = new UserController(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Add_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            var imageBytes = new byte[] { 1, 2, 3, 4 }; 
            var stream = new MemoryStream(imageBytes);
            var mockFile = new FormFile(stream, 0, stream.Length, "File", "testimage.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var viewModel = new AddViewModel2
            {
                Name = "test name",
                Address = "test address",
                Phone = "1234567890",
                Email = "test@gmail.com",
                File = mockFile
            };

            // Act
            var result = await _controller.Add(viewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Edit_ExistingUser_ShouldRedirectToIndex()
        {
            // Arrange
            var existingUser = new User
            {
                Id = 1,
                Name = "test name",
                Email = "test@gmail.com",
                Phone = "1234567890",
                Address = "test address"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(existingUser.Id)).ReturnsAsync(existingUser);

            var updatedImageBytes = new byte[] { 5, 6, 7, 8 }; 
            var updatedStream = new MemoryStream(updatedImageBytes);
            var updatedMockFile = new FormFile(updatedStream, 0, updatedStream.Length, "File", "updatedimage.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var updatedUser = new User
            {
                Id = 1,
                Name = "updated name",
                Email = "updated@gmail.com",
                Phone = "0987654321",
                Address = "updated address"
            };

            // Act
            var result = await _controller.Edit(updatedUser, updatedMockFile);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

           
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingUser_ShouldReturnNoContent()
        {
            // Arrange
            var userIdToDelete = 1;
            var existingUser = new User
            {
                Id = userIdToDelete,
                Name = "test name",
                Email = "test@gmail.com",
                Phone = "1234567890",
                Address = "test address"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userIdToDelete)).ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Delete(userIdToDelete);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);

          
            _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userIdToDelete), Times.Once);
        }
    }
}
