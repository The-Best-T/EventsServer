using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Server.ActionFilters;
using System.Collections.Generic;
using Xunit;
namespace Server.Tests.ActionFiltersTests
{
    public class ValidationFilterAttributeTests
    {
        private Mock<ILoggerManager> _loggerMock;

        [Fact]
        public void OnActionExecuting_TestResult_NotSetResult()
        {
            //Arrange
            var controller = new ValidationFilterAttribute(null);
            var context = GetContext(controller);

            context.ActionArguments.Add("Dto", "Dto");

            //Act
            controller.OnActionExecuting(context);

            //Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_TestResult_ReturnBadRequestObjectResult()
        {
            //Arrange
            _loggerMock = new Mock<ILoggerManager>();
            _loggerMock.Setup(l => l.LogError(It.IsAny<string>()));

            var controller = new ValidationFilterAttribute(_loggerMock.Object);
            var context = GetContext(controller);

            context.ActionArguments.Add("Dto", "");

            //Act
            controller.OnActionExecuting(context);

            //Assert
            Assert.IsType<BadRequestObjectResult>(context.Result);
        }

        [Fact]
        public void OnActionExecuting_TestResult_ReturnUnprocessableEntityObjectResult()
        {
            //Arrange
            _loggerMock = new Mock<ILoggerManager>();
            _loggerMock.Setup(l => l.LogError(It.IsAny<string>()));

            var controller = new ValidationFilterAttribute(_loggerMock.Object);
            var context = GetContext(controller);

            context.ActionArguments.Add("Dto", "Dto");
            context.ModelState.AddModelError("Error", "Error");

            //Act
            controller.OnActionExecuting(context);

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(context.Result);
        }

        private ActionExecutingContext GetContext(ValidationFilterAttribute controller)
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
            return context;
        }
    }
}