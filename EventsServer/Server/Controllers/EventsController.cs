using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Event;
using Entities.Models;
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

        [HttpGet("{id}",Name = "GetEventById")]
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

        [HttpPost]
        public ActionResult CreateEvent([FromBody] EventForCreationDto eventForCreationDto)
        {
            if (eventForCreationDto == null)
            {
                _logger.LogError("EventForCreationDto object sent from client is null.");
                return BadRequest("EventForCreationDto object is null");
            }

            var eventEntity = _mapper.Map<Event>(eventForCreationDto);

            _repository.Event.CreateEvent(eventEntity);
            _repository.Save();

            var eventToReturn = _mapper.Map<EventDto>(eventEntity);

            return CreatedAtRoute("GetEventById", new { id = eventToReturn.Id }, eventToReturn);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEventById(Guid id)
        {
            var deletedEvent = _repository.Event.GetEventById(id);
            if (deletedEvent==null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Event.DeleteEvent(deletedEvent);
            _repository.Save();

            return NoContent();
        }



    }
}
