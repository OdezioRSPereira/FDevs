using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class RespostaRepository : CrudRepository<long, EnqResposta>, IRespostaRepository
    {
        public RespostaRepository(IDbContext context) : base(context)
        {
        }
    }
}
