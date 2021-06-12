using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/respostas")]
    public class RespostaController : ControllerBase
    {
        private readonly IRespostaRepository _respostaRepository;

        public RespostaController(IRespostaRepository respostaRepository)
        {
            _respostaRepository = respostaRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<EnqQuiz> Obter([FromRoute] long id)
        {
            return Ok(_respostaRepository.Find(id));
        }

        [HttpPost]
        public ActionResult<EnqQuiz> Adicionar([FromBody] EnqResposta command)
        {
            return Created("respostas", _respostaRepository.Add(command));
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _respostaRepository.Remove(id);

            return NoContent();
        }
    }
}
