using Dapper.Contrib.Extensions;
using FDevsQuiz.Domain.Model.Base;
using System;

namespace FDevsQuiz.Domain.Model
{
    [Table(tableName: "App_Contato")]
    public class AppContato: BaseModel
    {
        [Key]
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string ImagemUrl { get; set; }
    }
}
