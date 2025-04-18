using MongoDB.Driver;
using UserService.Domain.Abstractions;

namespace UserService.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoClient _mongoClient;
        private IClientSessionHandle? _session;

        public UnitOfWork(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task BeginTransactionAsync()
        {
            _session = await _mongoClient.StartSessionAsync();
            _session.StartTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            if (_session == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await _session.CommitTransactionAsync();
            _session?.Dispose();
            _session = null;
        }

        public async Task AbortTransactionAsync()
        {
            if (_session == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await _session.AbortTransactionAsync();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
