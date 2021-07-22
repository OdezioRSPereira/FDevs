using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;

namespace FDevsQuiz.Domain.Repository
{
    public interface IRespostaRepository : ICrudRepository<long, EnqResposta>
    {
        void ExcluirResposta(long codigoContato, long codigoPergunta);
        RespostaQuery FindByPergunta(long codigoContato, long codigoPergunta);
    }
}
