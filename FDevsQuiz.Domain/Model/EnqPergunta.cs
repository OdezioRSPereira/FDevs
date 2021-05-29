using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "Enq_Pergunta")]
    public class EnqPergunta: BaseModel
    {
        [Key]
        public long Codigo { get; set; }
        public long CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public int OrdemExibicao { get; set; }
    }
}