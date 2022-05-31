using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Event;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.ActionFilters;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("modsen/events")]
    public class EventsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public EventsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpOptions]
        public IActionResult GetEventsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAllEvents([FromQuery] EventParameters eventParameters)
        {
            if (!eventParameters.ValidDateRange)
                return BadRequest("Max date can't be less than min date.");

            var events = await _repository.Event.GetAllEventsAsync(eventParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(events.MetaData));

            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return Ok(eventsDto);
        }

        [HttpGet("{id}", Name = "GetEventById")]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        public IActionResult GetEventById(Guid id)
        {
            var singleEvent = HttpContext.Items["checkEvent"] as Event;

            var eventDto = _mapper.Map<EventDto>(singleEvent);
            return Ok(eventDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        public async Task<IActionResult> DeleteEventByIdAsync(Guid id)
        {
            var deletedEvent = HttpContext.Items["checkEvent"] as Event;

            _repository.Event.DeleteEvent(deletedEvent);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        public async Task<IActionResult> UpdateEventByIdAsync(Guid id,
            [FromBody] EventForUpdateDto eventForUpdateDto)
        {
            var updateEvent = HttpContext.Items["checkEvent"] as Event;

            _mapper.Map(eventForUpdateDto, updateEvent);
            await _repository.SaveAsync();

            return NoContent();

        }

    }
}
