using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/perguntas")]
    public class PerguntaController : ControllerBase
    {
        private readonly IPerguntaRepository _perguntaRepository;

        public PerguntaController(IPerguntaRepository perguntaRepository)
        {
            _perguntaRepository = perguntaRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<EnqPergunta> Obter([FromRoute] long id)
        {
            return Ok(_perguntaRepository.Find(id));
        }

        [HttpPost]
        public ActionResult<EnqPergunta> Adicionar([FromBody] EnqPergunta command)
        {
            return Created("perguntas", _perguntaRepository.Add(command));
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromRoute] long id, [FromBody] EnqPergunta command)
        {
            var pergunta = _perguntaRepository.Find(id);
            pergunta.Titulo = command.Titulo;

            _perguntaRepository.Update(pergunta);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _perguntaRepository.Remove(id);

            return NoContent();
        }
    }
}
