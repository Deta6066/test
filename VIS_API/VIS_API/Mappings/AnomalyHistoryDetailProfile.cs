using AutoMapper;

namespace VIS_API.Mappings
{
    public class AnomalyHistoryDetailProfile : Profile
    {
        public AnomalyHistoryDetailProfile()
        {
            CreateMap<VisLibrary.Models.CNC.MAnomalyHistoryDetail, VisLibrary.Models.CNC.VMAnomalyHistoryDetail>()
                .ForMember(dest => dest.mach_name, opt => opt.MapFrom(src => src.mach_name))
                .ForMember(dest => dest.alarm_mesg, opt => opt.MapFrom(src => src.alarm_mesg))
                .ForMember(dest => dest.timespan, opt => opt.MapFrom(src => src.timespan));
        }
    }
}
