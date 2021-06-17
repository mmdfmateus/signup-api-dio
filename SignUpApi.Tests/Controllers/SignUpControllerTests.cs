using ApiValidation.Controllers;
using ApiValidation.Models;
using ApiValidation.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using SignUpApi.Validators;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SignUpApi.Tests.Controllers
{
    public class SignUpControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IEmailValidator> _emailValidatorMock;
        private readonly SignUpController _controller;

        private readonly SignUpRequest _validRequest;

        // Executado antes de cada teste
        public SignUpControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _emailValidatorMock = new Mock<IEmailValidator>();
            _emailValidatorMock.Setup(mock => mock.Validate(It.IsAny<string>()))  // Configurando o comportamento padrão do método Validate
                .Returns(true);

            _controller = new SignUpController(_userServiceMock.Object, _emailValidatorMock.Object);

            _validRequest = new SignUpRequest
            {
                Name = "Mateus",
                Email = "mateus@email.com",
                Password = "password",
                PasswordConfirmation = "password"
            };
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequestWhenPasswordIsDifferentThanPasswordConfirmation()
        {
            // Arrange
            var request = new SignUpRequest
            {
                Name = "Mateus",
                Email = "mateus@email.com",
                Password = "password",
                PasswordConfirmation = "differentPassword"
            };

            // Act
            var result = await _controller.CreateUserAsync(request);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;

            badRequest.Value.ShouldBe("O campo 'Password' e 'PasswordConfirmation' devem ser iguais");
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequestWhenEmailIsInvalid()
        {
            // Arrange
            _emailValidatorMock.Setup(mock => mock.Validate(It.IsAny<string>()))
                .Returns(false);
            var request = new SignUpRequest
            {
                Name = "Mateus",
                Email = "invalid_email",
                Password = "password",
                PasswordConfirmation = "password"
            };

            // Act
            var result = await _controller.CreateUserAsync(request);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;

            badRequest.Value.ShouldBe("O email fornecido não é valido");
        }

        [Fact]
        public async Task CreateUser_ShouldReturn500WhenEmailValidatorThrows()
        {
            // Arrange
            _emailValidatorMock.Setup(mock => mock.Validate(It.IsAny<string>()))
                .Throws(new Exception("Some error occurred"));
            var controller = new SignUpController(_userServiceMock.Object, _emailValidatorMock.Object);

            // Act
            var result = await controller.CreateUserAsync(_validRequest);

            // Assert
            result.ShouldBeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;

            objectResult.StatusCode.ShouldBe(500);
        }

        [Fact]
        public async Task CreateUser_ShouldCallEmailValidatorWithCorrectParam()
        {
            // Act
            var result = await _controller.CreateUserAsync(_validRequest);

            // Assert
            _emailValidatorMock.Verify(
                mock => mock.Validate(_validRequest.Email),
                Times.Once
            );
        }
    }
}
