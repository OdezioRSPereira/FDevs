using System;
using System.Data;

namespace FDevsQuiz.Domain.Interface
{
    public interface IDbContext: IDisposable
    {
        IDbConnection Connection { get; }
    }
}
