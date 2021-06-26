namespace FDevsQuiz.Domain.Command
{
    public class RegistroCommand
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string ImagemUrl { get; set; }
        public string Senha { get; set; }
    }
}
