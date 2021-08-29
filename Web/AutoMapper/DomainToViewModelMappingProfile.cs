using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Web.ViewModels;
using AutoMapper;
using System.Linq;

namespace AspNetCoreApiSample.Web.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Usuario, UsuarioResponseViewModel>();
            CreateMap<UsuarioPermissao, PermissaoSistemaEnum>().ConstructUsing(e => e.Permissao);
            CreateMap<UsuarioQueryResponse, UsuarioResponseViewModel>();
            CreateMap<UsuarioQueryResponseGetFilteredList, UsuarioGetFilteredListResponseViewModel>();
        }
    }
}
