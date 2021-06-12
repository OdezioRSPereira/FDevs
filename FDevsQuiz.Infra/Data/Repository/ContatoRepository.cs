using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class ContatoRepository : CrudRepository<long, AppContato>, IContatoRepository
    {
        public ContatoRepository(IDbContext context) : base(context)
        {
        }
    }
}
