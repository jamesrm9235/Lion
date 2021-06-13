using AutoMapper;

namespace Lion.Server
{
    public sealed class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Abstractions.Bundle, Models.Bundle>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BundleId));

            CreateMap<Abstractions.Message, Models.Message>();
        }
    }
}
