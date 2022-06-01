using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.User;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<ILoggerManager> _loggerMock;
        private Mock<IAuthenticationManager> _authenticationMock;
        private Mock<UserManager<User>> _userManagerMock;

        [Fact]
        public async Task RegisterUser_TestResult_ReturnStatusCode201()
        {
            //Arrange
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(mp => mp.Map<User>(It.IsAny<UserForRegistrationDto>()));

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new AuthenticationController(null, _mapperMock.Object, _userManagerMock.Object, null);

            //Act
            var result = await controller.RegisterUser(new UserForRegistrationDto());
            var resultCode = result as StatusCodeResult;

            //Assert
            Assert.NotNull(resultCode);
            Assert.Equal(201, resultCode?.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_TestResult_ReturnUnprocessableEntity()
        {
            //Arrange
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(mp => mp.Map<User>(It.IsAny<UserForRegistrationDto>()));

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new AuthenticationController(null, _mapperMock.Object, _userManagerMock.Object, null);

            //Act
            var result = await controller.RegisterUser(new UserForRegistrationDto());

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_TestResult_ReturnOkObject()
        {
            //Arrange
            _authenticationMock = new Mock<IAuthenticationManager>();
            _authenticationMock.Setup(a => a.ValidateUser(It.IsAny<UserForAuthenticationDto>()))
                .Returns(Task.FromResult(true));
            _authenticationMock.Setup(a => a.CreateToken());

            var controller = new AuthenticationController(null, null, null, _authenticationMock.Object);

            //Act
            var result = await controller.Authenticate(It.IsAny<UserForAuthenticationDto>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_TestResult_ReturnUnauthorized()
        {
            //Arrange
            _authenticationMock = new Mock<IAuthenticationManager>();
            _authenticationMock.Setup(a => a.ValidateUser(It.IsAny<UserForAuthenticationDto>()))
                .Returns(Task.FromResult(false));

            _loggerMock = new Mock<ILoggerManager>();
            _loggerMock.Setup(l => l.LogWarn(It.IsAny<string>()));

            var controller = new AuthenticationController(_loggerMock.Object, null, null, _authenticationMock.Object);

            //Act
            var result = await controller.Authenticate(It.IsAny<UserForAuthenticationDto>());

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}
