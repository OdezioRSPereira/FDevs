using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;
using System.Collections.Generic;

namespace FDevsQuiz.Domain.Repository
{
    public interface IPerguntaRepository: ICrudRepository<long, EnqPergunta>
    {

        IEnumerable<EnqPergunta> FindCodigoQuiz(long codigoQuiz);
        QuizPontuacaoQuery ObterPontuacao(long codigoContato);
    }
}
