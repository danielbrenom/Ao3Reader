using Ao3Domain.Models.Data;
using Ao3Domain.Models.Response;
using AutoMapper;

namespace Ao3Reader.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkResponse, WorkIndexing>().ReverseMap();
            CreateMap<Work, WorkIndexing>().ReverseMap();
        }
    }
}