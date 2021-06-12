using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using System.Collections.Generic;

namespace FDevsQuiz.Domain.Repository
{
    public interface IQuizRepository : ICrudRepository<long, EnqQuiz>
    {
        IEnumerable<EnqQuiz> FindAll();
    }
}
