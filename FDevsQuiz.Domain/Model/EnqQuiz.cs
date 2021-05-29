using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "Enq_Quiz")]
    public class EnqQuiz: BaseModel
    {
        [Key]
        public long Codigo { get; set; }
        public string Titulo { get; set; }
        public int CodigoNivel { get; set; }
        public string ImagemUrl { get; set; }
    }
}
