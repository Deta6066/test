using AutoMapper;
using VIS_API.Models.CNC;
using VIS_API.Models.View;
namespace VIS_API.Mappings
{
    public class MachineMonitoringDashboardProfile: Profile
    {
        public MachineMonitoringDashboardProfile()
        {
            CreateMap<MMachineInfo, VMMachineMonitoringDashboard>()
                .ForMember(dest => dest.MachName, opt => opt.MapFrom(src => src.mach_name));
        }
    }
}
