using GQ.Data.dto;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace GQ.Sql
{
    public interface IBaseService : IDisposable
    {
        void Migrate();

        ISessionMapper<T> GetSession<T>() where T : class, IEntity, new();

        IDbContextTransaction BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();

    }
}
