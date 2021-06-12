using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
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
    }
}
