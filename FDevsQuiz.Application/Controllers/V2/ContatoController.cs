using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/contatos")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<AppContato> Obter([FromRoute] long id)
        {
            return Ok(_contatoRepository.Find(id));
        }

        [HttpPost]
        public ActionResult<AppContato> Adicionar([FromBody] AppContato command)
        {
            return Created("contatos", _contatoRepository.Add(command));
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromRoute] long id, [FromBody] AppContato command)
        {
            var contato = _contatoRepository.Find(id);
            contato.Nome = command.Nome;
            contato.SobreNome = command.SobreNome;
            contato.DataNascimento = command.DataNascimento;
            contato.Email = command.Email;
            contato.Telefone = command.Telefone;
            contato.ImagemUrl = command.ImagemUrl;

            _contatoRepository.Update(contato);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _contatoRepository.Remove(id);

            return NoContent();
        }
    }
}
