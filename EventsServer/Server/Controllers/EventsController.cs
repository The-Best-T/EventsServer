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
            try
            {
                var events = _repository.Event.GetAllEvents(trackChanges: false);
                var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);
                return Ok(eventsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllEvents)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
