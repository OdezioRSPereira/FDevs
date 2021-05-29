using System.Collections.Generic;

namespace FDevsQuiz.Application.Command
{
    public class PerguntaCommand
    {
        public string Titulo { get; set; }
        public ICollection<AlternativaCommand> Alternativas { get; set; }
    }
}