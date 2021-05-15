using System.Collections.Generic;

namespace FDevsQuiz.Model
{
    public class Quiz
    {
        public long Codigo { get; set; }
        public string Titulo { get; set; }
        public long Respostas { get; set; }
        public string Nivel { get; set; }
        public string ImagemUrl { get; set; }
        public ICollection<Pergunta> Perguntas { get; set; }
    }
}
