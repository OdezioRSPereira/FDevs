using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "Enq_Alternativa")]
    public class EnqAlternativa: BaseModel
    {
        [Key]
        public long Codigo { get; set; }
        public long CodigoPergunta { get; set; }
        public string Titulo { get; set; }
        public bool Correta { get; set; }
        public int OrdemExibicao { get; set; }
    }
}