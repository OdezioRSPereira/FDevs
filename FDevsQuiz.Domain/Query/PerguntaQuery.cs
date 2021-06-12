using System.Collections.Generic;

namespace FDevsQuiz.Domain.Query
{
    public class PerguntaQuery
    {
        public long Codigo { get; set; }
        public string Titulo { get; set; }
        public ICollection<AlternativaQuery> Alternativas { get; set; }
    }
}