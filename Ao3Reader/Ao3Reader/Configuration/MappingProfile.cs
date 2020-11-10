using Ao3Domain.Models.Data;
using Ao3Domain.Models.Response;
using Ao3Reader.Models;
using AutoMapper;

namespace Ao3Reader.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkResponse, WorkIndexing>().ReverseMap();
            CreateMap<Work, WorkIndexing>().ReverseMap();
            CreateMap<ChapterListing, ChapterListingView>().ReverseMap();
        }
    }
}