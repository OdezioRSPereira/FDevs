using System;

namespace FDevsQuiz.Domain.Model.Base
{
    public abstract class BaseModel
    {
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
