using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Query;
using FDevsQuiz.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FDevsQuiz.Application.Controllers.V3
{
    [Controller]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet()]
        public ActionResult<ICollection<QuizQuery>> ObterTodos()
        {
            return Ok(_quizService.FindAll());
        }

        [HttpGet("{id}")]
        public ActionResult<QuizQuery> Obter([FromRoute] long id)
        {
            return Ok(_quizService.Find(id));
        }

        [HttpPost]
        public ActionResult<QuizQuery> Adicionar([FromBody] AddQuizCommand command)
        {
            return Created("quizzes", _quizService.Add(command));
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromRoute] long id, [FromBody] AddQuizCommand command)
        {
            _quizService.Update(id, command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _quizService.Remove(id);

            return NoContent();
        }
    }
}
