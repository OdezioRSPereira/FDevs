using FDevsQuiz.Domain.Enumerable;
using System.Collections.Generic;

namespace FDevsQuiz.Domain.Command
{
    public class AddQuizCommand
    {
        public string Titulo { get; set; }
        public ENivel Nivel { get; set; }
        public string ImagemUrl { get; set; }
        public ICollection<AddPerguntaCommand> Perguntas { get; set; }
    }
}
