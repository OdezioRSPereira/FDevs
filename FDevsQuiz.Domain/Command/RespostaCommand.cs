namespace FDevsQuiz.Domain.Command
{
    public class RespostaCommand
    {
        public long CodigoPergunta { get; set; }
        public long? CodigoAlternativa { get; set; }
    }
}
