using FDevsQuiz.Domain.Enumerable;
using System.Collections.Generic;

namespace FDevsQuiz.Domain.Query
{
    public class QuizQuery
    {
        public long Codigo { get; set; }
        public string Titulo { get; set; }
        public ENivel Nivel { get; set; }
        public string ImagemUrl { get; set; }
        public ICollection<PerguntaQuery> Perguntas { get; set; }
    }
}
