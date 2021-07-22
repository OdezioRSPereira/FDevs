using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;
using System.Collections.Generic;
using System.Text;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class AlternativaRepository : CrudRepository<long, EnqAlternativa>, IAlternativaRepository
    {
        public AlternativaRepository(IDbContext context) : base(context)
        {
        }

        public bool Exists(long codigoPergunta, long codigoAlternativa)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select (Case When Count(0) = 0 Then 0 Else 1 End) As Existe ");
            sql.Append("   From Enq_Pergunta per ");
            sql.Append("  Inner Join Enq_Alternativa alt on (alt.CodigoPergunta = per.Codigo) ");
            sql.Append("  Where per.Codigo = @CodigoPergunta ");
            sql.Append("    And alt.Codigo = @CodigoAlternativa ");
            #endregion

            return QuerySingleOrDefault<bool>(sql, new
            {
                codigoPergunta,
                codigoAlternativa
            });
        }

        public IEnumerable<EnqAlternativa> FindCodigoPergunta(long codigoPergunta)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select * ");
            sql.Append("   From Enq_Alternativa with(nolock)");
            sql.Append("  Where CodigoPergunta = @CodigoPergunta ");
            #endregion

            return Query<EnqAlternativa>(sql, new
            {
                codigoPergunta
            });
        }
    }
}
