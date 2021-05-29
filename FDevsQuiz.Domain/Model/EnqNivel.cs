using Dapper.Contrib.Extensions;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "Enq_Nivel")]
    public class EnqNivel
    {
        [ExplicitKey]
        public int Codigo { get; set; }
        public string Descricao { get; set; }
    }
}