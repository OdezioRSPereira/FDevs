using FDevsQuiz.Application.Controllers.Base;
using FDevsQuiz.Application.Identity;
using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;
using FDevsQuiz.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FDevsQuiz.Application.Controllers
{
    [Authorize]
    [Route("autenticacao")]
    public class AutenticacaoController : BaseController
    {
        private readonly IUsuario _usuario;
        private readonly IConfiguration _configuration;
        private readonly AutenticacaoService _autenticacaoService;

        public AutenticacaoController(
            IUsuario usuario,
            IConfiguration configuration,
            AutenticacaoService autenticacaoService)
        {
            _usuario = usuario;
            _configuration = configuration;
            _autenticacaoService = autenticacaoService;
        }

        private AcessoQuery GerarToken(AppContato contato)
        {
            return new AcessoQuery
            {
                Email = contato.Email,
                AccessToken = JwtToken.GenerateToken(_configuration, contato)
            };
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<ActionResult<AcessoQuery>> Registrar([FromBody] RegistroCommand command)
        {
            var contato = await _autenticacaoService.Registrar(command);
            return Ok(GerarToken(contato));
        }

        [AllowAnonymous]
        [HttpPost("acessar")]
        public async Task<ActionResult<AcessoQuery>> Acessar([FromBody] AcessoCommand command)
        {
            var contato = await _autenticacaoService.Autenticacao(command);
            return Ok(GerarToken(contato));
        }

        [HttpGet("me")]
        public async Task<ActionResult<dynamic>> Me()
        {
            return await Task.FromResult(new
            {
               _usuario.Codigo,
               _usuario.Nome
            });
        }

    }
}
