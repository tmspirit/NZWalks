using AutoMapper;

namespace NZWalksAPI.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile() {
            //Aqui Automapper hace el mapeo de region a nuestro DTO
            //ORIGEN,DESTINO
            CreateMap<Models.Domain.Region, Models.DTO.Region>();
            CreateMap<Models.Domain.Region, Models.DTO.UpdateRegionRequest>().ReverseMap();
            CreateMap<Models.Domain.Region, Models.DTO.UpdateRegionRequest>();
            //Aqui le decimos el mapeo cuando los nombre no coinciden, primero el destino y luego el origen
            //.ForMember(dest=> dest.Id, options => options.MapFrom(src => src.Id));

        }
    }
}
