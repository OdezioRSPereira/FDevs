using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "App_Usuario")]
    public class AppUsuario: BaseModel
    {
        [ExplicitKey]
        public long Codigo { get; set; }
        public string Senha { get; set; }
    }
}
