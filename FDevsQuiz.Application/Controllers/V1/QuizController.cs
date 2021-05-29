using FDevsQuiz.Application.Command;
using FDevsQuiz.Application.Controllers.V1.Base;
using FDevsQuiz.Application.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;                    

namespace FDevsQuiz.Application.Controllers.V1
{
    [Controller]
    [Route("v1/quizzes")]
    public class QuizController : AbstractController<long, Quiz>
    {

        protected override string Filename => "quizzes.json";

        protected override Quiz Item(ICollection<Quiz> dados, long id)
        {
            return dados.Where(q => q.Codigo == id).FirstOrDefault();
        }

        protected override Quiz GerarChave(ICollection<Quiz> dados, Quiz model)
        {
            model.Codigo = dados.Select(q => q.Codigo).ToList().Max() + 1;
            return model;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Quiz>>> Quizzes()
        {
            return Ok(await Dados());
        }

        [HttpGet("{id}")]
        public ActionResult<Quiz> Quiz([FromRoute] long id)
        {
            return Ok(FindById(id));
        }

        [HttpPost]
        public async Task<ActionResult<Quiz>> Adicionar([FromBody] QuizCommand command)
        {
            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Titulo do quiz é obrigatório");

            if (string.IsNullOrEmpty(command.Nivel))
                throw new Exception("Nível do quiz é obrigatório");

            if ((command.Perguntas == null) || (command.Perguntas.Count == 0))
                throw new Exception("O quiz deve conter pelo menos uma pergunta");

            if (command.Perguntas.Where(p => p.Alternativas?.Count != 4).Count() > 0)
                throw new Exception("As perguntas do quiz devem conter 4 alternativas");

            var quizzes = await Dados();

            var quiz = new Quiz()
            {
                Titulo = command.Titulo,
                Nivel = command.Nivel,
                ImagemUrl = command.ImagemUrl,
                Perguntas = new List<Pergunta>()
            };

            for (var i = 0; i < command.Perguntas.Count; i++)
            {
                var perguntaCommand = command.Perguntas.ElementAt(i);

                if (string.IsNullOrEmpty(perguntaCommand.Titulo))
                    throw new Exception($"A pergunta {i} não possui título");

                var pergunta = new Pergunta()
                {
                    Titulo = perguntaCommand.Titulo,
                    Alternativas = new List<Alternativa>()
                };

                var corretas = perguntaCommand.Alternativas.Where(a => a.Correta == true).Count();
                if (corretas == 0)
                    throw new Exception($"A pergunta {i} não possui uma alternativa correta");
                else if (corretas > 1)
                    throw new Exception($"A pergunta {i} possui mais de uma alternativa correta");

                foreach (var alternativaCommand in perguntaCommand.Alternativas)
                {
                    if (string.IsNullOrEmpty(alternativaCommand.Titulo))
                        throw new Exception($"A pergunta {i} possui alternativa sem título");

                    var alternativa = new Alternativa()
                    {
                        Titulo = alternativaCommand.Titulo,
                        Correta = alternativaCommand.Correta
                    };

                    pergunta.Alternativas.Add(alternativa);
                }

                quiz.Perguntas.Add(pergunta);
            }

            quiz = await Adicionar(quiz);

            return Created("quizzes/{id}", quiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] long id, [FromBody] AtualizarQuizCommand command)
        {
            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Titulo do quiz é obrigatório");

            if (string.IsNullOrEmpty(command.Nivel))
                throw new Exception("Nível do quiz é obrigatório");

            var quizzes = await Dados();
            var quiz = quizzes.Where(u => u.Codigo == id).FirstOrDefault();

            if (quiz == null)
                return NotFound("Quiz não encontrado.");

            quiz.Titulo = command.Titulo;
            quiz.Nivel = command.Nivel;
            quiz.ImagemUrl = command.ImagemUrl;

            await SalvarDadosAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] long id)
        {
            await RemoveById(id);

            return NoContent();
        }
    }
}
