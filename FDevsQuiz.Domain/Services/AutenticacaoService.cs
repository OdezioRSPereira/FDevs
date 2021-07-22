using FDevsQuiz.Domain.Command;
using FDevsQuiz.Domain.Exceptions;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FDevsQuiz.Domain.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IContatoRepository _contatoRepository;

        public AutenticacaoService(
            IUsuarioRepository usuarioRepository,
            IContatoRepository contatoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _contatoRepository = contatoRepository;
        }

        public async Task<AppContato> Registrar(RegistroCommand command)
        {
            if ((string.IsNullOrEmpty(command.Nome)) || (string.IsNullOrEmpty(command.SobreNome)))
                throw new ValidateException("Nome e sobrenome obrigatório para o cadastro");

            if ((string.IsNullOrEmpty(command.Email)) || (string.IsNullOrEmpty(command.Senha)))
                throw new ValidateException("Email e senha obrigatório para o cadastro");

            var contato = _contatoRepository.FindByEmail(command.Email);
            if (contato != null)
                throw new ValidateException("Já existe um usuário para o email informado");

            using var scope = new TransactionScope();
            contato = _contatoRepository.Add(new AppContato
            {
                Nome = command.Nome,
                SobreNome = command.SobreNome,
                ImagemUrl = command.ImagemUrl,
                Email = command.Email,
            });

            _usuarioRepository.Add(new AppUsuario
            {
                Codigo = contato.Codigo,
                Senha = Convert.ToBase64String(Encoding.ASCII.GetBytes(command.Senha))
            });

            scope.Complete();

            return await Task.FromResult(contato);
        }

        public async Task<AppContato> Autenticacao(AcessoCommand command)
        {
            if ((string.IsNullOrEmpty(command.Email)) || (string.IsNullOrEmpty(command.Senha)))
                throw new ValidateException("Email e senha obrigatório para login");

            var contato = _contatoRepository.FindByEmail(command.Email);
            if (contato == null)
                throw new ValidateException("Usuário ou senha inválido");

            var usuario = _usuarioRepository.Find(contato.Codigo);

            if ((usuario == null) || (usuario.Senha != Convert.ToBase64String(Encoding.ASCII.GetBytes(command.Senha))))
                throw new ValidateException("Usuário ou senha inválido");

            return await Task.FromResult(contato);
        }
    }
}
