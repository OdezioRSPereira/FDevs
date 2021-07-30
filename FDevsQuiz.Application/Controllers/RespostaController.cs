using FDevsQuiz.Application.Controllers.Base;
using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers
{
    [Authorize]
    [Route("respostas")]
    public class RespostaController : BaseController
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
