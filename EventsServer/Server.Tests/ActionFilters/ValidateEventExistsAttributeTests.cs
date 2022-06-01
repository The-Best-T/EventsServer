using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Server.ActionFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests.ActionFiltersTests
{
    public class ValidateEventExistsAttributeTests
    {
        private Mock<ILoggerManager> _loggerMock;
        private Mock<IRepositoryManager> _repositoryMock;

        [Fact]
        public async Task OnActionExecutionAsync_TestResult_GoToAction()
        {
            //Arrange
            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.GetEventByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new Event()));

            var controller = new ValidateEventExistsAttribute(_repositoryMock.Object, null);

            var (context, next) = GetContextAndDelegate(controller);

            context.ActionArguments.Add("id", It.IsAny<Guid>());

            //Act
            await controller.OnActionExecutionAsync(context, next);

            //Asser
            Assert.NotNull(context.HttpContext.Items["EndOfTest"]);
        }

        [Fact]
        public async Task OnActionExecutionAsync_TestHttpItems_SetFridge()
        {
            //Arrange
            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.GetEventByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new Event()));

            var controller = new ValidateEventExistsAttribute(_repositoryMock.Object, null);

            var (context, next) = GetContextAndDelegate(controller);

            context.ActionArguments.Add("id", It.IsAny<Guid>());

            //Act
            await controller.OnActionExecutionAsync(context, next);

            //Asser
            Assert.NotNull(context.HttpContext.Items["checkEvent"]);
        }

        [Fact]
        public async Task OnActionExecutionAsync_TestResult_NotFound()
        {
            //Arrange
            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.GetEventByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult((Event)null));

            _loggerMock = new Mock<ILoggerManager>();
            _loggerMock.Setup(l => l.LogInfo(It.IsAny<string>()));

            var controller = new ValidateEventExistsAttribute(_repositoryMock.Object, _loggerMock.Object);

            var (context, next) = GetContextAndDelegate(controller);

            context.ActionArguments.Add("id", It.IsAny<Guid>());

            //Act
            await controller.OnActionExecutionAsync(context, next);

            //Asser
            Assert.IsType<NotFoundResult>(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_TestRepository_GetFridgeAsyncMustBeCalled()
        {
            //Arrange
            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.GetEventByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new Event()));

            var controller = new ValidateEventExistsAttribute(_repositoryMock.Object, null);

            var (context, next) = GetContextAndDelegate(controller);

            context.ActionArguments.Add("id", It.IsAny<Guid>());

            //Act
            await controller.OnActionExecutionAsync(context, next);

            //Asser
            _repositoryMock.VerifyAll();
        }

        private (ActionExecutingContext, ActionExecutionDelegate) GetContextAndDelegate(ValidateEventExistsAttribute controller)
        {
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            };
            var metadata = new List<IFilterMetadata>();

            var context = new ActionExecutingContext(
                actionContext,
                metadata,
                new Dictionary<string, object>(),
                controller);

            ActionExecutionDelegate next = () =>
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, controller);
                context.HttpContext.Items.Add("EndOfTest", 125);
                return Task.FromResult(ctx);
            };

            return (context, next);
        }
    }
}