using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/alternativas")]
    public class AlternativaController : ControllerBase
    {
        private readonly IAlternativaRepository _alternativaRepository;

        public AlternativaController(IAlternativaRepository alternativaRepository)
        {
            _alternativaRepository = alternativaRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<EnqAlternativa> Obter([FromRoute] long id)
        {
            return Ok(_alternativaRepository.Find(id));
        }

        [HttpPost]
        public ActionResult<EnqAlternativa> Adicionar([FromBody] EnqAlternativa command)
        {
            return Created("alternativa", _alternativaRepository.Add(command));
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromRoute] long id, [FromBody] EnqAlternativa command)
        {
            var alternativa = _alternativaRepository.Find(id);
            alternativa.Titulo = command.Titulo;
            alternativa.Correta = command.Correta;
            alternativa.OrdemExibicao = command.OrdemExibicao;

            _alternativaRepository.Update(alternativa);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _alternativaRepository.Remove(id);

            return NoContent();
        }
    }
}
