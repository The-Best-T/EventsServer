using AutoMapper;
using Entities.DataTransferObjects.Event;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDto>();
            CreateMap<EventForCreationDto, Event>();
        }
    }
}
