using AutoMapper;
using Entities.DataTransferObjects.User;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegistrationDto, User>();  
        }
    }
}
