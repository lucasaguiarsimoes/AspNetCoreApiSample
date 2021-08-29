using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Queries;
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
            CreateMap<UsuarioGetFilteredListRequestViewModel, UsuarioQueryGetFilteredList>();

            CreateMap<LoginViewModel, LoginCommand>();
        }
    }
}
