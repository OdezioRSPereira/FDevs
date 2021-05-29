using FDevsQuiz.Application.Command;
using FDevsQuiz.Application.Controllers.V1.Base;
using FDevsQuiz.Application.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDevsQuiz.Application.Controllers.V1
{
    [Controller]
    [Route("v1/usuarios")]
    public class UsuarioController : AbstractController<long, Usuario>
    {
        protected override string Filename => "usuarios.json";

        protected override Usuario Item(ICollection<Usuario> dados, long id)
        {
            return dados.Where(u => u.CodigoUsuario == id).FirstOrDefault();
        }

        protected override Usuario GerarChave(ICollection<Usuario> dados, Usuario model)
        {
            model.CodigoUsuario = dados.Select(u => u.CodigoUsuario).ToList().Max() + 1;
            return model;
        }

        /// <summary>
        /// Retorna os usuários cadastrados na aplicação
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ICollection<Usuario>>> Usuarios()
        {
            return Ok(await Dados());
        }

        /// <summary>
        /// Localiza um usuário cadastrado na  aplicação através do identificador
        /// </summary>
        /// <param name="id">Código do usuário</param>
        [HttpGet("{id}")]
        public ActionResult<Usuario> Usuario([FromRoute] long id)
        {
            return Ok(FindById(id));
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Adicionar([FromBody] UsuarioCommand command)
        {
            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nome do usuário obrigatório");

            var usuario = await Adicionar(new Usuario
            {
                NomeUsuario = command.NomeUsuario,
                Pontuacao = 0,
                ImagemUrl = command.ImagemUrl
            });

            return Created("usuarios/{id}", usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] long id, [FromBody] UsuarioCommand command)
        {
            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nome do usuário obrigatório");

            var usuario = FindById(id);
            
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            usuario.NomeUsuario = command.NomeUsuario;
            usuario.ImagemUrl = command.ImagemUrl;

            await SalvarDadosAsync();

            return NoContent();
        }

        [HttpPut("{id}/pontuacao")]
        public async Task<IActionResult> Pontuacao([FromRoute] long id, [FromBody] PontuacaoCommand command)
        {
            var usuario = FindById(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado");

            usuario.Pontuacao = command.Pontuacao;

            await SalvarDadosAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] long id)
        {
            await RemoveById(id);

            return NoContent();
        }
    }
}
