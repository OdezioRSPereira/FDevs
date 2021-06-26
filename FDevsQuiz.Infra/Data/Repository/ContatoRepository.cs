using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;
using System.Text;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class ContatoRepository : CrudRepository<long, AppContato>, IContatoRepository
    {
        public ContatoRepository(IDbContext context) : base(context)
        {
        }

        public AppContato FindByEmail(string email)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select * ");
            sql.Append("   From App_Contato with(nolock)");
            sql.Append("  Where Email = @Email");
            #endregion

            return QuerySingleOrDefault<AppContato>(sql, new { email });
        }
    }
}
