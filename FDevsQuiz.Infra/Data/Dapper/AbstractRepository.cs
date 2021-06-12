using Dapper;
using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDevsQuiz.Infra.Data.Dapper
{
    public abstract class AbstractRepository<ID, TEntity> where TEntity : class
    {
        private readonly IDbContext _context;

        protected AbstractRepository(IDbContext context)
        {
            _context = context;
        }

        private TEntity NewInstance(ID id)
        {
            var key = DapperHelper<TEntity>.FieldKey().FirstOrDefault();
            var model = (TEntity)Activator.CreateInstance(typeof(TEntity));
            key.SetValue(model, id);

            return model;
        }

        public IEnumerable<TQuery> Query<TQuery>(StringBuilder sql, object param = null)
        {
            return _context.Connection.Query<TQuery>(sql.ToString(), param);
        }

        public TQuery QuerySingleOrDefault<TQuery>(StringBuilder sql, object param = null)
        {
            return _context.Connection.QuerySingleOrDefault<TQuery>(sql.ToString(), param);
        }

        public TQuery QueryFirstOrDefault<TQuery>(StringBuilder sql, object param = null)
        {
            return _context.Connection.QueryFirstOrDefault<TQuery>(sql.ToString(), param);
        }


        public void Execute(StringBuilder sql, object param = null)
        {
            _context.Connection.Execute(sql.ToString(), param);
        }

        public TEntity Find(TEntity model)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append($" Select * ");
            sql.Append($"   From {DapperHelper<TEntity>.TableName()} With(nolock)  ");
            sql = DapperHelper<TEntity>.Condition(sql);
            #endregion

            return _context.Connection.QuerySingleOrDefault<TEntity>(sql.ToString(), model); ;
        }

        public TEntity Find(ID id)
        {
            return Find(NewInstance(id));
        }

        public bool Exists(TEntity model)
        {
            #region [ Sql ]
            var sql = new StringBuilder();
            sql.Append($" Select Case Count(0) When 0 Then 0 Else 1 End as Existe ");
            sql.Append($"   From {DapperHelper<TEntity>.TableName()} With(nolock) ");
            sql = DapperHelper<TEntity>.Condition(sql);
            #endregion

            return _context.Connection.QuerySingleOrDefault<bool>(sql.ToString(), model);
        }

        public bool Exists(ID id)
        {
            return Exists(NewInstance(id));
        }

        public TEntity Add(TEntity model) 
        {
            if (model is BaseModel baseModel)
            {
                baseModel.DataCadastro = DateTime.Now;
                baseModel.DataAtualizacao = baseModel.DataCadastro;
            }

            _context.Connection.Insert(model);

            return model;
        }

        public TEntity Update(TEntity model)
        {
            if (model is BaseModel baseModel)
                baseModel.DataAtualizacao = DateTime.Now;

            _context.Connection.Update(model);

            return model;
        }

        public bool Remove(TEntity model)
        {
            return _context.Connection.Delete(model);
        }

        public bool Remove(ID id)
        {
            return Remove(NewInstance(id));
        }
    }

}
