using AutoMapper;
using VIS_API.Models;

namespace VIS_API.Mappings
{
    public class AssembleCenterProfile:Profile
    {
        public AssembleCenterProfile()
        {
            CreateMap<MAssembleCenter, VAssembleCenter>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => (src.sName??"").Trim()));
        }
    }
}
