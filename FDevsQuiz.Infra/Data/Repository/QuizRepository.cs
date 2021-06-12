using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;
using System.Collections.Generic;
using System.Text;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class QuizRepository : CrudRepository<long, EnqQuiz>, IQuizRepository
    {
        public QuizRepository(IDbContext context) : base(context)
        {
        }

        public IEnumerable<EnqQuiz> FindAll()
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append(" Select * ");
            sql.Append("   From Enq_Quiz with(nolock)");
            #endregion

            return Query<EnqQuiz>(sql);
        }
    }
}
