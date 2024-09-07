namespace System.Pdv.Core.Interfaces;

public interface ITransactionManager
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
