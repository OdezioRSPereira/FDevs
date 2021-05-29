using System.Collections.Generic;

namespace FDevsQuiz.Application.Command
{
    public class QuizCommand
    {
        public string Titulo { get; set; }
        public string Nivel { get; set; }
        public string ImagemUrl { get; set; }
        public ICollection<PerguntaCommand> Perguntas { get; set; }
    }
}
