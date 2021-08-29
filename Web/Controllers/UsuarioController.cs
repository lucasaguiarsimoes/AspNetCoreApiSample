using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Service.Interface;
using AspNetCoreApiSample.Web.Attributes;
using AspNetCoreApiSample.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.Controllers
{
    [ApiController]
    [AuthorizeWithRole]
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IMapper mapper, IUsuarioService usuarioService)
        {
            this._mapper = mapper;
            this._usuarioService = usuarioService;
        }

        [HttpGet("{id}")]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioConsulta)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseViewModel>>> GetById(long id, CancellationToken cancellationToken)
        {
            // Aciona a busca através do service
            Usuario? usuario = await this._usuarioService.GetByIdAsync(new UsuarioQueryGetById() { ID = id }, cancellationToken);

            // Se não encontrar, retorna um status diferenciado
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Monta o ViewModel a partir da busca realizada
            return Ok(this._mapper.Map<UsuarioResponseViewModel>(usuario));
        }

        [HttpPost]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioConsulta)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseViewModel>>> GetFilteredList(UsuarioGetFilteredListRequestViewModel request, CancellationToken cancellationToken)
        {
            // Cria mapeamento da ViewModel para objeto
            UsuarioQueryGetFilteredList query = this._mapper.Map<UsuarioQueryGetFilteredList>(request);

            // Aciona a busca através do service
            IEnumerable<UsuarioQueryResponseGetFilteredList> usuarios = await this._usuarioService.GetFilteredListAsync(query, cancellationToken);

            // Monta o ViewModel a partir da busca realizada
            return Ok(usuarios.Select(c => this._mapper.Map<UsuarioGetFilteredListResponseViewModel>(c)));
        }

        [HttpGet]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioConsulta)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            // Aciona a busca através do service
            IEnumerable<UsuarioQueryResponse> usuarios = await this._usuarioService.GetAllAsync(cancellationToken);

            // Monta o ViewModel a partir da busca realizada
            return Ok(usuarios.Select(c => this._mapper.Map<UsuarioResponseViewModel>(c)));
        }

        [HttpPost]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioCria)]
        public async Task<IActionResult> Create([FromBody] UsuarioAddViewModel usuarioViewModel, CancellationToken cancellationToken)
        {
            // Monta o Command a partir do ViewModel
            UsuarioCommandAdd addUsuarioCommand = this._mapper.Map<UsuarioCommandAdd>(usuarioViewModel);

            // Aciona o Command pelo service
            long ID = await this._usuarioService.AddAsync(addUsuarioCommand, cancellationToken);

            // Aplica o Action para o retorno
            return CreatedAtAction(nameof(GetById), new { id = ID });
        }

        [HttpPut]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioEdita)]
        public async Task<IActionResult> Edit([FromBody] UsuarioEditViewModel usuarioViewModel, CancellationToken cancellationToken)
        {
            // Monta o Command a partir do ViewModel
            UsuarioCommandEdit editUsuarioCommand = this._mapper.Map<UsuarioCommandEdit>(usuarioViewModel);

            // Aciona o Command pelo service
            await this._usuarioService.EditAsync(editUsuarioCommand, cancellationToken);

            // Aplica o Action para o retorno
            return Ok();
        }

        [HttpDelete]
        [AuthorizeWithRole(PermissaoSistemaEnum.UsuarioRemove)]
        public async Task<IActionResult> Remove([FromBody] UsuarioRemoveViewModel usuarioViewModel, CancellationToken cancellationToken)
        {
            // Monta o Command a partir do ViewModel
            UsuarioCommandRemove removeUsuarioCommand = this._mapper.Map<UsuarioCommandRemove>(usuarioViewModel);

            // Aciona o Command pelo service
            await this._usuarioService.RemoveAsync(removeUsuarioCommand, cancellationToken);

            // Aplica o Action para o retorno
            return NoContent();
        }
    }
}
