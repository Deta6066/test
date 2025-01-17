using AutoMapper;
using VisLibrary.Models.CNC;
namespace VIS_API.Mappings
{
    public class MMachineImageProfile:Profile
    {
        public MMachineImageProfile()
        {
            CreateMap<MMachineImage, VMMachineImage>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.company_fk, opt => opt.MapFrom(src => src.company_fk))
                .ForMember(dest => dest.area_fk, opt => opt.MapFrom(src => src.area_fk)).ForMember(dest=> dest.image_data, opt => opt.MapFrom(src => src.image_data)).ForMember(dest => dest.image_data_base64, opt => opt.MapFrom(src =>Convert.ToBase64String(src.image_data))).ForMember(dest => dest.image_type, opt => opt.MapFrom(src => src.image_type)).ForMember(dest => dest.mach_id, opt => opt.MapFrom(src => src.mach_id)

                );

        }
    }

}
