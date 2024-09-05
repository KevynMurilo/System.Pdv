namespace System.Pdv.Core.Interfaces;

public interface IUnitOfWorkRepository
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
