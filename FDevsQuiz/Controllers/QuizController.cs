using FDevsQuiz.Model;
using FDevsQuiz.Command;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace FDevsQuiz.Controllers
{
    [Controller]
    [Route("quizzes")]
    public class QuizController : ControllerBase
    {
        public string Filename => "quizzes.json";

        private string Fullname { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", Filename); }

        protected async Task<ICollection<T>> CarregarDadosAsync<T>()
        {
            using var openStream = System.IO.File.OpenRead(Fullname);
            return await JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        private async Task SalvarDadosAsync<T>(ICollection<T> dados)
        {
            using FileStream createStream = System.IO.File.Create(Fullname);
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Quiz>>> Quizzes()
        {
            return Ok(await CarregarDadosAsync<Quiz>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> Quiz([FromRoute] long id)
        {
            var quizzes = await CarregarDadosAsync<Quiz>();
            var quiz = quizzes.Where(u => u.Codigo == id).FirstOrDefault();
            return Ok(quiz);
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

            var quizzes = await CarregarDadosAsync<Quiz>();

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

                var correta = perguntaCommand.Alternativas.Where(a => a.Correta == true).Count();
                if (correta == 0)
                    throw new Exception($"A pergunta {i} não possui uma alternativa correta");
                else if (correta > 1)
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

            // encontra ultimo codigo e incrementa 1 para gerar novo codigo
            quiz.Codigo = quizzes.Select(q => q.Codigo).ToList().Max() + 1;

            quizzes.Add(quiz);

            await SalvarDadosAsync(quizzes);

            return Created("quizzes/{id}", quiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar([FromRoute] long id, [FromBody] AtualizarQuizCommand command)
        {
            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Titulo do quiz é obrigatório");

            if (string.IsNullOrEmpty(command.Nivel))
                throw new Exception("Nível do quiz é obrigatório");

            var quizzes = await CarregarDadosAsync<Quiz>();
            var quiz = quizzes.Where(u => u.Codigo == id).FirstOrDefault();

            if (quiz == null)
                return NotFound("Quiz não encontrado.");

            quiz.Titulo = command.Titulo;
            quiz.Nivel = command.Nivel;
            quiz.ImagemUrl = command.ImagemUrl;

            await SalvarDadosAsync(quizzes);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir([FromRoute] long id)
        {
            var quizzes = await CarregarDadosAsync<Quiz>();
            quizzes = quizzes.Where(u => u.Codigo != id).ToList();

            await SalvarDadosAsync(quizzes);

            return NoContent();
        }
    }
}
