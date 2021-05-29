namespace FDevsQuiz.Domain.Interface
{
    public interface IReadRepository<ID, TEntity> where TEntity : class
    {
        bool Exists(TEntity model);
        bool Exists(ID id);
        TEntity Find(TEntity model);
        TEntity Find(ID id);
    }
}
