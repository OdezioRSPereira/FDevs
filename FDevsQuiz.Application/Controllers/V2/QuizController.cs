using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<EnqQuiz> Obter([FromRoute] long id)
        {
            return Ok(_quizRepository.Find(id));
        }

        [HttpPost]
        public ActionResult<EnqQuiz> Adicionar([FromBody] EnqQuiz command)
        {
            return Created("quizzes", _quizRepository.Add(command));
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromRoute] long id, [FromBody] EnqQuiz command)
        {
            var quiz = _quizRepository.Find(id);
            quiz.Titulo = command.Titulo;
            quiz.CodigoNivel = command.CodigoNivel;
            quiz.ImagemUrl = command.ImagemUrl;

            _quizRepository.Update(quiz);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir([FromRoute] long id)
        {
            _quizRepository.Remove(id);

            return NoContent();
        }
    }
}
