using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Web.ViewModels;
using AutoMapper;

namespace AspNetCoreApiSample.Web.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UsuarioAddViewModel, UsuarioCommandAdd>();
            CreateMap<UsuarioEditViewModel, UsuarioCommandEdit>();
            CreateMap<UsuarioRemoveViewModel, UsuarioCommandRemove>();

            CreateMap<LoginViewModel, LoginCommand>();
        }
    }
}
