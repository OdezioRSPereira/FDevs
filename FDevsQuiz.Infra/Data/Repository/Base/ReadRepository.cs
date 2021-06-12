using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Infra.Data.Dapper;

namespace FDevsQuiz.Infra.Data.Repository.Base
{
    public abstract class ReadRepository<ID, TEntity> : AbstractRepository<ID, TEntity>, IReadRepository<ID, TEntity> where TEntity : class
    {
        protected ReadRepository(IDbContext context) : base(context)
        {
        }

    }
}
