using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, User>();
            CreateMap<DateTime, DateTime>().ConvertUsing((s, d) => {
                return DateTime.SpecifyKind(s, DateTimeKind.Utc);
            });
        }
    }
}