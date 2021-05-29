using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FDevsQuiz.Infra.Data.Dapper
{
    public static class DapperHelper<TEntity>
    {

        public static string TableName()
        {
            return typeof(TEntity).GetCustomAttribute<TableAttribute>(false)?.Name ?? throw new ArgumentException("Entity must have one [Table] property");
        }

        public static IEnumerable<PropertyInfo> FieldKey()
        {
            return typeof(TEntity).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);
        }

        public static StringBuilder Condition(StringBuilder sql)
        {
            var operador = " Where";

            var keys = FieldKey();
            foreach (var key in keys)
            {
                sql.Append($"{operador} {key.Name} = @{key.Name}");
                operador = "   And";
            }

            return sql;
        }
    }
}
