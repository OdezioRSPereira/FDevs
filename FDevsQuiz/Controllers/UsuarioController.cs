using FDevsQuiz.Command;
using FDevsQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FDevsQuiz.Controllers
{
    [Controller]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        public string Filename => "usuarios.json";

        private string Fullname { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", Filename); }

        protected async Task<ICollection<T>> CarregarDadosAsync<T>()
        {
            using var openStream = System.IO.File.OpenRead(Fullname);
            return await JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        private async Task SalvarDadosAsync<T>(ICollection<T> dados)
        {
            using FileStream createStream = System.IO.File.Create(Fullname);
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Usuario>>> Usuarios()
        {
            return Ok(await CarregarDadosAsync<Usuario>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Usuario([FromRoute] long id)
        {
            var usuarios = await CarregarDadosAsync<Usuario>();
            return Ok(usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Adicionar([FromBody] UsuarioCommand command)
        {
            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nome do usuário obrigatório");

            var usuario = new Usuario
            {
                NomeUsuario = command.NomeUsuario,
                Pontuacao = 0,
                ImagemUrl = command.ImagemUrl
            };

            var usuarios = await CarregarDadosAsync<Usuario>();
            // encontra ultimo codigo e incrementa 1 para gerar novo codigo
            usuario.CodigoUsuario = usuarios.Select(u => u.CodigoUsuario).ToList().Max() + 1;

            usuarios.Add(usuario);

            await SalvarDadosAsync(usuarios);

            return Created("usuarios/{id}", usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] long id, [FromBody] UsuarioCommand command)
        {
            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nome do usuário obrigatório");

            var usuarios = await CarregarDadosAsync<Usuario>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();
            
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            usuario.NomeUsuario = command.NomeUsuario;
            usuario.ImagemUrl = command.ImagemUrl;

            await SalvarDadosAsync(usuarios);

            return NoContent();
        }

        [HttpPut("{id}/pontuacao")]
        public async Task<IActionResult> Pontuacao([FromRoute] long id, [FromBody] PontuacaoCommand command)
        {
            var usuarios = await CarregarDadosAsync<Usuario>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();

            if (usuario == null)
                return NotFound("Usuário não encontrado");

            usuario.Pontuacao = command.Pontuacao;

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
