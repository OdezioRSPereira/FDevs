using FDevsQuiz.Command;
using FDevsQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDevsQuiz.Controllers
{
    [Controller]
    [Route("usuarios")]
    public class UsuarioController : BaseController
    {
        public override string Filename => "usuarios.json";

        [HttpGet]
        public async Task<ActionResult<ICollection<dynamic>>> Quizzes()
        {
            var usuarios = await CarregarDadosAsync<dynamic>();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Quiz([FromRoute] long id)
        {
            var usuarios = await CarregarDadosAsync<Usuario>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Adicionar([FromBody] AdicionarUsuarioCommand command)
        {
            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nomer do usuário obrigatório");

            var usuarios = await CarregarDadosAsync<Usuario>();

            var usuario = new Usuario
            {
                NomeUsuario = command.NomeUsuario,
                Pontuacao = 0,
                ImagemUrl = command.ImagemUrl
            };

            // encontra o maior numero do codigo do usuario e incrementa 1 para o novo codigo
            usuario.CodigoUsuario = usuarios.Select(u => u.CodigoUsuario).ToList().Max() + 1;

            usuarios.Add(usuario);

            await SalvarDadosAsync(usuarios);

            return Created("usuarios/{id}", usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] long id, [FromBody] AtualizarUsuarioCommand command)
        {
            var usuarios = await CarregarDadosAsync<Usuario>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();
            
            if (usuario == null)
                return NotFound();

            usuario.NomeUsuario = command.NomeUsuario;
            usuario.Pontuacao = command.Pontuacao;
            usuario.ImagemUrl = command.ImagemUrl;

            await SalvarDadosAsync(usuarios);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] long id)
        {
            var usuarios = await CarregarDadosAsync<Usuario>();
            usuarios = usuarios.Where(u => u.CodigoUsuario != id).ToList();

            await SalvarDadosAsync(usuarios);

            return NoContent();
        }

    }
}
