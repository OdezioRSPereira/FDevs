using FDevsQuiz.Domain.Interface;

namespace FDevsQuiz.Infra.Data.Repository.Base
{
    public abstract class CrudRepository<ID, TEntity> : ReadRepository<ID, TEntity>, ICrudRepository<ID, TEntity> where TEntity : class
    {
        protected CrudRepository(IDbContext context) : base(context)
        {
        }

    }
}
