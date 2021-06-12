namespace FDevsQuiz.Domain.Command
{
    public class AtualizarQuizCommand
    {
        public string Titulo { get; set; }
        public long Respostas { get; set; }
        public string Nivel { get; set; }
        public string ImagemUrl { get; set; }
    }
}
