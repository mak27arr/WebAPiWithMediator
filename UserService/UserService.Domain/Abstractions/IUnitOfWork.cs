namespace UserService.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task AbortTransactionAsync();
    }
}
