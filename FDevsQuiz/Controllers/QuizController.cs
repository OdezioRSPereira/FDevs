using FDevsQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FDevsQuiz.Controllers
{
    [Controller]
    [Route("quizzes")]
    public class QuizController : BaseController
    {
        public override string Filename => "quizzes.json";

        [HttpGet]
        public async Task<ActionResult<ICollection<dynamic>>> Quizzes()
        {
            var quizzes = await CarregarDadosAsync<dynamic>();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> Quizzes([FromRoute] long id)
        {
            var quizzes = await CarregarDadosAsync<Quiz>();
            var quiz = quizzes.Where(u => u.Codigo == id).FirstOrDefault();
            return Ok(quiz);
        }
    }
}
