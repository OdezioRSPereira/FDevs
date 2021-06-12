namespace FDevsQuiz.Domain.Interface
{
    public interface ICrudRepository<ID, TEntity> : IReadRepository<ID, TEntity>
    {
        TEntity Add(TEntity model);

        bool Remove(TEntity model);

        bool Remove(ID id);
        
        TEntity Update(TEntity model);
    }
}
