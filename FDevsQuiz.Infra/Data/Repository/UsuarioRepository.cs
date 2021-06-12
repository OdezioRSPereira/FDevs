using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class UsuarioRepository : CrudRepository<long, AppUsuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDbContext context) : base(context)
        {
        }
    }
}
