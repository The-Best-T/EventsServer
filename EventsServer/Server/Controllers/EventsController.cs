using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Event;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Server.ActionFilters;

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
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _repository.Event.GetAllEventsAsync();
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);
            return Ok(eventsDto);
        }

        [HttpGet("{id}", Name = "GetEventById")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var singleEvent = await _repository.Event.GetEventByIdAsync(id);
            if (singleEvent == null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var eventDto = _mapper.Map<EventDto>(singleEvent);
            return Ok(eventDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventForCreationDto eventForCreationDto)
        {
            var eventEntity = _mapper.Map<Event>(eventForCreationDto);

            _repository.Event.CreateEvent(eventEntity);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(eventEntity);

            return CreatedAtRoute("GetEventById", new { id = eventToReturn.Id }, eventToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventByIdAsync(Guid id)
        {
            var deletedEvent = await _repository.Event.GetEventByIdAsync(id);
            if (deletedEvent == null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Event.DeleteEvent(deletedEvent);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEventByIdAsync(Guid id,
            [FromBody] EventForUpdateDto eventForUpdateDto)
        {
            var updateEvent = await _repository.Event.GetEventByIdAsync(id, true);
            if (updateEvent == null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(eventForUpdateDto, updateEvent);
            await _repository.SaveAsync();

            return NoContent();

        }

    }
}
