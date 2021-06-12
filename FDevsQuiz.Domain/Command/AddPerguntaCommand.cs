using System.Collections.Generic;

namespace FDevsQuiz.Domain.Command
{
    public class AddPerguntaCommand
    {
        public string Titulo { get; set; }
        public ICollection<AddAlternativaCommand> Alternativas { get; set; }
    }
}