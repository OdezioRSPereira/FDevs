using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;

namespace FDevsQuiz.Domain.Repository
{
    public interface IRespostaRepository: ICrudRepository<long, EnqResposta>
    {
    }
}
