using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V3
{
    [Authorize]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/respostas")]
    public class RespostaController : ControllerBase
    {
        private readonly QuizService _quizService;

        public RespostaController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] RespostaCommand command)
        {
            _quizService.AdicionarResposta(command);

            return NoContent();
        }

    }
}
