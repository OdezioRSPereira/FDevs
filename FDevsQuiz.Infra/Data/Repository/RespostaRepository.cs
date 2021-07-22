using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Query;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;
using System.Text;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class RespostaRepository : CrudRepository<long, EnqResposta>, IRespostaRepository
    {
        public RespostaRepository(IDbContext context) : base(context)
        {
        }

        public void ExcluirResposta(long codigoContato, long codigoPergunta)
        {
            #region [ SQL ]
            var sql = new StringBuilder();
            sql.Append(" Delete From Enq_Resposta ");
            sql.Append("  Where CodigoContato = @CodigoContato ");
            sql.Append("    And CodigoAlternativa in (Select Codigo  ");
            sql.Append("                                From Enq_Alternativa ");
            sql.Append(" 						       Where CodigoPergunta = @CodigoPergunta) ");
            #endregion

            Execute(sql, new
            {
                codigoContato,
                codigoPergunta
            });
        }

        public RespostaQuery FindByPergunta(long codigoContato, long codigoPergunta)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select CodigoAlternativa  ");
            sql.Append("   From Enq_Resposta ");
            sql.Append("  Where CodigoContato = @CodigoContato ");
            sql.Append("    And CodigoPergunta = @CodigoPergunta ");
            #endregion

            return QuerySingleOrDefault<RespostaQuery>(sql, new
            {
                codigoContato,
                codigoPergunta
            });
        }
    }
}
