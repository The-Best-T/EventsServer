using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Event;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("modsen/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public EventsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAllEvents()
        {
            var events = _repository.Event.GetAllEvents();
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);
            return Ok(eventsDto);
        }

        [HttpGet("{id}")]
        public ActionResult GetEventById(Guid id)
        {
            var singleEvent = _repository.Event.GetEventById(id);
            if (singleEvent == null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var eventDto = _mapper.Map<EventDto>(singleEvent);
            return Ok(eventDto);
        }

    }
}
