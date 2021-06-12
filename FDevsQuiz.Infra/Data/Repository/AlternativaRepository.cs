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
