using System.Collections.Generic;

namespace FDevsQuiz.Model
{
    public class Pergunta
    {
        public string Titulo { get; set; }
        public ICollection<Alternativa> Alternativas { get; set; }
    }
}