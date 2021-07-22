using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;
using System.Collections.Generic;
using System.Text;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class PerguntaRepository : CrudRepository<long, EnqPergunta>, IPerguntaRepository
    {
        public PerguntaRepository(IDbContext context) : base(context)
        {
        }

        public IEnumerable<EnqPergunta> FindCodigoQuiz(long codigoQuiz)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select * ");
            sql.Append("   From Enq_Pergunta with(nolock)");
            sql.Append("  Where CodigoQuiz = @CodigoQuiz ");
            #endregion

            return Query<EnqPergunta>(sql, new
            {
                codigoQuiz
            });
        }

        public QuizPontuacaoQuery ObterPontuacao(long codigoContato)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select Sum(res.Respostas) As Respostas, ");
            sql.Append("        Sum(res.Acertos) As Acertos,  ");
            sql.Append("        Count(0) As Total          ");
            sql.Append("   From Enq_Pergunta per  ");
            sql.Append("  Outer Apply (Select Count(0) As Respostas, ");
            sql.Append("                      Sum(Case alt.Correta When 1 Then 1 Else 0 End) As Acertos ");
            sql.Append("                 From Enq_Resposta res  ");
            sql.Append("                Inner Join Enq_Alternativa alt on (alt.Codigo = res.CodigoAlternativa) ");
            sql.Append(" 			   Where res.CodigoContato = @CodigoContato ");
            sql.Append(" 			     And alt.CodigoPergunta = per.Codigo) res  ");
            #endregion

            return QuerySingleOrDefault<QuizPontuacaoQuery>(sql, new
            {
                codigoContato
            });
        }
    }
}
