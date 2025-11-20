using System.Threading.Tasks;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Application.Security;
using Agenda.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Agenda.Api.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public void Login_Should_Return_Token_For_Valid_Admin_User()
        {
            // Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            jwtServiceMock.Setup(j => j.GenerateToken(It.IsAny<string>(), It.IsAny<IEnumerable<System.Security.Claims.Claim>>()))
                .Returns("dummy_token");

            var controller = new AuthController(jwtServiceMock.Object);
            var request = new LoginRequestDto
            {
                Username = "admin",
                Password = "P@ssw0rd"
            };

            // Act
            var result = controller.Login(request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var ok = result.Result as OkObjectResult;
            ok!.Value.Should().BeOfType<LoginResponseDto>();
            var response = ok.Value as LoginResponseDto;
            response!.Token.Should().Be("dummy_token");
            response.Username.Should().Be("admin");
        }

        [Fact]
        public void Login_Should_Return_Unauthorized_For_Invalid_User()
        {
            // Arrange
            var jwtServiceMock = new Mock<IJwtTokenService>();
            var controller = new AuthController(jwtServiceMock.Object);
            var request = new LoginRequestDto
            {
                Username = "invalid",
                Password = "wrong"
            };

            // Act
            var result = controller.Login(request);

            // Assert
            result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}
