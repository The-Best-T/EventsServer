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
        /// <summary>
        /// Return all acces requests for this url
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all acces requests for this url</response>
        /// <response code="500">Server error</response>
        [HttpOptions]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetEventsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        /// <summary>
        /// Get list of all events
        /// </summary>
        /// <param name="eventParameters">Filter parameters</param>
        /// <returns>The events list</returns>
        /// <response code="200">Returns the list of all events</response>
        /// <response code="400">Request parameters are not valid</response>
        /// <response code="500">Server error</response>

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEvents([FromQuery] EventParameters eventParameters)
        {
            if (!eventParameters.ValidDateRange)
                return BadRequest("Max date can't be less than min date.");

            var events = await _repository.Event.GetAllEventsAsync(eventParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(events.MetaData));

            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return Ok(eventsDto);
        }
        /// <summary>
        /// Return one event by id
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns>Return one event</returns>
        /// <response code="200">Returns one event</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Event with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{id}", Name = "GetEventById")]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetEventById(Guid id)
        {
            var singleEvent = HttpContext.Items["checkEvent"] as Event;

            var eventDto = _mapper.Map<EventDto>(singleEvent);
            return Ok(eventDto);
        }
        /// <summary>
        /// Create one new event
        /// </summary>
        /// <param name="eventForCreationDto">New event</param>
        /// <returns></returns>
        /// <response code="201">Returns one new event</response>
        /// <response code="400">New event is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="422">New event not valid</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventForCreationDto eventForCreationDto)
        {
            var eventEntity = _mapper.Map<Event>(eventForCreationDto);

            _repository.Event.CreateEvent(eventEntity);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(eventEntity);

            return CreatedAtRoute("GetEventById", new { id = eventToReturn.Id }, eventToReturn);
        }
        /// <summary>
        /// Delete event by id 
        /// </summary>
        /// <param name="id">event id</param>
        /// <returns></returns>
        /// <response code="204">Event deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Event with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        public async Task<IActionResult> DeleteEventByIdAsync(Guid id)
        {
            var deletedEvent = HttpContext.Items["checkEvent"] as Event;

            _repository.Event.DeleteEvent(deletedEvent);
            await _repository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Update event by id
        /// </summary>
        /// <param name="id">event id</param>
        /// <param name="eventForUpdateDto">new event</param>
        /// <returns></returns>
        /// <response code="204">Event updated</response>
        /// <response code="400">New event is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Event with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEventExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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
