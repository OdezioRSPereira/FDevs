using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Enumerable;
using FDevsQuiz.Domain.Exceptions;
using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;
using FDevsQuiz.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FDevsQuiz.Domain.Services
{
    public class QuizService
    {
        private readonly IUsuario _usuario;
        private readonly IQuizRepository _quizRepository;
        private readonly IPerguntaRepository _perguntaRepository;
        private readonly IRespostaRepository _respostaRepository;
        private readonly IAlternativaRepository _alternativaRepository;

        public QuizService(
            IUsuario usuario,
            IQuizRepository quizRepository,
            IPerguntaRepository perguntaRepository,
            IRespostaRepository respostaRepository,
            IAlternativaRepository alternativaRepository)
        {
            _usuario = usuario;
            _quizRepository = quizRepository;
            _perguntaRepository = perguntaRepository;
            _respostaRepository = respostaRepository;
            _alternativaRepository = alternativaRepository;
        }

        public QuizPontuacaoQuery ObterPontuacao()
        {
            return _perguntaRepository.ObterPontuacao(_usuario.Codigo.Value);
        }

        public void AdicionarResposta(RespostaCommand command)
        {
            if (command.CodigoAlternativa.HasValue)
            {
                if (!_alternativaRepository.Exists(command.CodigoPergunta, command.CodigoAlternativa.Value))
                    throw new ValidateException("Alternativa não encontrada.");
            } else if (!_perguntaRepository.Exists(command.CodigoPergunta))
                throw new ValidateException("Pergunta não encontrada.");

            _respostaRepository.ExcluirResposta(_usuario.Codigo.GetValueOrDefault(0), command.CodigoPergunta);

            _respostaRepository.Add(new EnqResposta
            {
                CodigoPergunta = command.CodigoPergunta,
                CodigoAlternativa = command.CodigoAlternativa,
                CodigoContato = _usuario.Codigo.GetValueOrDefault(0)
            });
        }

        public ICollection<QuizQuery> FindAll()
        {
            var queries = new List<QuizQuery>();

            var quizzes = _quizRepository.FindAll();
            foreach (var quiz in quizzes)
            {
                var query = QuizToQuery(quiz);
                query.Perguntas = CarregarPerguntas(query.Codigo);
                queries.Add(query);
            }

            return queries;
        }

        private ICollection<PerguntaQuery> CarregarPerguntas(long codigoQuiz)
        {
            var queries = new List<PerguntaQuery>();
            var perguntas = _perguntaRepository.FindCodigoQuiz(codigoQuiz);
            foreach (var pergunta in perguntas)
            {
                var query = PerguntaToQuery(pergunta);
                var alternativas = _alternativaRepository.FindCodigoPergunta(pergunta.Codigo);
                foreach (var alternativa in alternativas)
                {
                    query.Alternativas.Add(AlternativaToQuery(alternativa));
                }

                queries.Add(query);
            }

            return queries;
        }


        public QuizQuery Find(long id)
        {
            var quiz = QuizToQuery(_quizRepository.Find(id));
            if (quiz == null)
                return null;

            quiz.Perguntas = CarregarPerguntas(quiz.Codigo);

            return quiz;
        }

        private static QuizQuery QuizToQuery(EnqQuiz quiz)
        {
            if (quiz == null)
                return null;

            return new QuizQuery
            {
                Codigo = quiz.Codigo,
                Titulo = quiz.Titulo,
                Nivel = (ENivel)quiz.CodigoNivel,
                ImagemUrl = quiz.ImagemUrl,
                Perguntas = new List<PerguntaQuery>()
            };
        }

        private PerguntaQuery PerguntaToQuery(EnqPergunta pergunta)
        {
            var resposta = _respostaRepository.FindByPergunta(_usuario.Codigo.Value, pergunta.Codigo);

            return new PerguntaQuery
            {
                Codigo = pergunta.Codigo,
                Titulo = pergunta.Titulo,
                Resposta = resposta,
                Alternativas = new List<AlternativaQuery>()
            };
        }

        private static AlternativaQuery AlternativaToQuery(EnqAlternativa alternativa)
        {
            return new AlternativaQuery
            {
                Codigo = alternativa.Codigo,
                Titulo = alternativa.Titulo,
                Correta = alternativa.Correta
            };
        }

        private ICollection<AlternativaQuery> AddAlternativa(long codigoPergunta, ICollection<AddAlternativaCommand> commands)
        {
            var alternativas = new List<AlternativaQuery>();
            foreach (var command in commands)
            {
                var alternativa = _alternativaRepository.Add(new EnqAlternativa
                {
                    CodigoPergunta = codigoPergunta,
                    Titulo = command.Titulo,
                    Correta = command.Correta
                });

                alternativas.Add(AlternativaToQuery(alternativa));
            }

            return alternativas;
        }

        private ICollection<PerguntaQuery> AddPerguntas(long codigoQuiz, ICollection<AddPerguntaCommand> commands)
        {
            var perguntas = new List<PerguntaQuery>();
            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands.ElementAt(i);

                if (string.IsNullOrEmpty(command.Titulo))
                    throw new Exception($"A pergunta {i} não possui título");

                var pergunta = _perguntaRepository.Add(new EnqPergunta
                {
                    CodigoQuiz = codigoQuiz,
                    Titulo = command.Titulo,
                    OrdemExibicao = i
                });

                var query = PerguntaToQuery(pergunta);

                var corretas = command.Alternativas.Where(a => a.Correta == true).Count();
                if (corretas == 0)
                    throw new Exception($"A pergunta {i} não possui uma alternativa correta");
                else if (corretas > 1)
                    throw new Exception($"A pergunta {i} possui mais de uma alternativa correta");

                if (command.Alternativas.Where(a => string.IsNullOrEmpty(a.Titulo)).Any())
                    throw new Exception($"A pergunta '{i}' possui alternativa sem título");


                query.Alternativas = AddAlternativa(pergunta.Codigo, command.Alternativas);

                perguntas.Add(query);
            }

            return perguntas;
        }

        public QuizQuery Add(AddQuizCommand command)
        {
            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Titulo do quiz é obrigatório");

            if (string.IsNullOrEmpty(command.Nivel.ToString()))
                throw new Exception("Nível do quiz é obrigatório");

            if ((command.Perguntas == null) || (command.Perguntas.Count == 0))
                throw new Exception("O quiz deve conter pelo menos uma pergunta");

            if (command.Perguntas.Where(p => p.Alternativas?.Count != 4).Any())
                throw new Exception("As perguntas do quiz devem conter 4 alternativas");

            var quiz = _quizRepository.Add(new EnqQuiz
            {
                Titulo = command.Titulo,
                CodigoNivel = (int)command.Nivel,
                ImagemUrl = command.ImagemUrl
            });

            var query = QuizToQuery(quiz);
            query.Perguntas = AddPerguntas(quiz.Codigo, command.Perguntas);

            return query;
        }

        public void Update(long id, AddQuizCommand command)
        {
            var quiz = _quizRepository.Find(id);
            quiz.Titulo = command.Titulo;
            quiz.CodigoNivel = (int)command.Nivel;
            quiz.ImagemUrl = command.ImagemUrl;

            _quizRepository.Update(quiz);
        }

        public void Remove(long id)
        {
            var perguntas = _perguntaRepository.FindCodigoQuiz(id);
            foreach(var pergunta in perguntas)
            {
                var alternativas = _alternativaRepository.FindCodigoPergunta(pergunta.Codigo);
                foreach(var alternativa in alternativas)
                {
                    _alternativaRepository.Remove(alternativa);
                }

                _perguntaRepository.Remove(pergunta);
            }

            _quizRepository.Remove(id);
        }
    }
}
