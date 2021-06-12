using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using System.Collections.Generic;

namespace FDevsQuiz.Domain.Repository
{
    public interface IAlternativaRepository : ICrudRepository<long, EnqAlternativa>
    {
        IEnumerable<EnqAlternativa> FindCodigoPergunta(long codigoPergunta);
    }
}
