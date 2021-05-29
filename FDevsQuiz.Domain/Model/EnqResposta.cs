using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "Enq_Resposta")]
    public class EnqResposta: BaseModel
    {
        [Key]
        public long Codigo { get; set; }
        public long CodigoContato { get; set; }
        public long CodigoAlternativa { get; set; }
    }
}