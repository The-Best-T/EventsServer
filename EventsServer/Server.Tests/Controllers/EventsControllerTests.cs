using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Event;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests.Controllers
{
    public class EventsControllerTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IRepositoryManager> _repositoryMock;

        [Fact]
        public async Task GetAllEvents_TestResult_ReturnOkObjectResult()
        {
            //Arrange
            var pagedList = new PagedList<Event>(GetListOfEvents(), 3, 1, 3);
            var eventsDtoList = GetListOfEventsDto();
            EventParameters eventParameters = new EventParameters
            {
                PageNumber = 1,
                MinDate = DateTime.MinValue,
                MaxDate = DateTime.MaxValue,
                SearchName = ""
            };

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(
                mp => mp.Map<IEnumerable<EventDto>>(pagedList))
                        .Returns(eventsDtoList);

            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(
                rp => rp.Event
                .GetAllEventsAsync(eventParameters, false))
                .Returns(Task.FromResult(pagedList));

            var controller = new EventsController(_repositoryMock.Object, _mapperMock.Object,null);
            SetContext(controller);

            //Act
            var result = await controller.GetAllEvents(eventParameters);
            var okObjectResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okObjectResult);
            Assert.Equal(eventsDtoList, okObjectResult?.Value);
        }

        [Fact]
        public async Task GetAllEvents_TestResult_ReturnBadRequest()
        {
            //Arrange 
            EventParameters eventParameters = new EventParameters
            {
                PageNumber = 1,
                MinDate = DateTime.MaxValue,
                MaxDate = DateTime.MinValue,
                SearchName = ""
            };

            var controller = new EventsController(null, null,null);

            //Act
            var result = await controller.GetAllEvents(eventParameters);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetEventById_TestResult_ReturnOkObjectResult()
        {
            //Arrange
            _mapperMock = new Mock<IMapper>();

            var controller = new EventsController(null, _mapperMock.Object,null);
            SetContext(controller);

            //Act
            var result = controller.GetEventById(It.IsAny<Guid>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateEvent_TestResult_ReturnStatusCreated()
        {
            //Arrange
            var eventDto = GetListOfEventsDto()[0];

            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.CreateEvent(It.IsAny<Event>()));

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(
                mp => mp.Map<EventDto>(It.IsAny<EventForCreationDto>()))
                        .Returns(eventDto);

            var controller = new EventsController(_repositoryMock.Object, _mapperMock.Object,null);

            //Act
            var result = await controller.CreateEventAsync(It.IsAny<EventForCreationDto>());

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(eventDto, ((CreatedAtRouteResult)result).Value);
        }

        [Fact]
        public async Task DeleteEventById_TestResult_ReturnStatusNoContent()
        {
            //Arrange
            _repositoryMock = new Mock<IRepositoryManager>();
            _repositoryMock.Setup(rp => rp.Event.DeleteEvent(It.IsAny<Event>()));

            var controller = new EventsController(_repositoryMock.Object, null,null);
            SetContext(controller);

            //Act
            var result = await controller.DeleteEventByIdAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_TestResult_ReturnStatusNoContent()
        {
            //Arrange
            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(mp => mp.Map(It.IsAny<EventForUpdateDto>(), It.IsAny<Event>()));

            _repositoryMock = new Mock<IRepositoryManager>();

            var controller = new EventsController(_repositoryMock.Object, _mapperMock.Object,null);
            SetContext(controller);

            //Act
            var result = await controller.UpdateEventByIdAsync(It.IsAny<Guid>(), It.IsAny<EventForUpdateDto>());

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        private void SetContext(EventsController controller)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }
        private List<Event> GetListOfEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Wedding",
                    Description = "Wedding of Maxim and Anna",
                    Speaker = "Holy Father Peter",
                    Place = "North Church",
                    Date = new DateTime(2022, 7, 20)
                },
                new Event
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Name = "Birthday",
                    Description = "Birthday of Elena",
                    Speaker = "Clown Anton",
                    Place = "Hot bar",
                    Date = new DateTime(2022, 8, 29)
                },
                new Event
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Name = "Olympiad in programming",
                    Description = "Minsk olympiad in programming",
                    Speaker = "Genadiy Andreevich",
                    Place = "School 32",
                    Date = new DateTime(2022, 9, 21)
                }
            };
        }
        private List<EventDto> GetListOfEventsDto()
        {
            return new List<EventDto>
            {
                 new EventDto
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Wedding",
                    Description = "Wedding of Maxim and Anna",
                    Speaker = "Holy Father Peter",
                    Place = "North Church",
                    Date = new DateTime(2022, 7, 20)
                },
                new EventDto
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Name = "Birthday",
                    Description = "Birthday of Elena",
                    Speaker = "Clown Anton",
                    Place = "Hot bar",
                    Date = new DateTime(2022, 8, 29)
                },
                new EventDto
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Name = "Olympiad in programming",
                    Description = "Minsk olympiad in programming",
                    Speaker = "Genadiy Andreevich",
                    Place = "School 32",
                    Date = new DateTime(2022, 9, 21)
                }
            };
        }
    }
}
