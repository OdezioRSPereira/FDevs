using System;
using System.Collections.Generic;
using System.Text;

namespace FDevsQuiz.Domain.Interface
{
    public interface ICrudRepository<ID, TEntity> : IReadRepository<ID, TEntity> where TEntity : class
    {
        TEntity Add(TEntity model);

        bool Remove(TEntity model);

        bool Remove(ID id);

        TEntity Update(TEntity model);
    }
}
