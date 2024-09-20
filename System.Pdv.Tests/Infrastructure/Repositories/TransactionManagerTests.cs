using Moq;
using Xunit;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class TransactionManagerTests
{
    private readonly Mock<ITransactionManager> _transactionManagerMock;

    public TransactionManagerTests()
    {
        _transactionManagerMock = new Mock<ITransactionManager>();
    }

    [Fact]
    public async Task BeginTransactionAsync_ShouldBeginTransaction()
    {
        await _transactionManagerMock.Object.BeginTransactionAsync();
        _transactionManagerMock.Verify(tm => tm.BeginTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CommitTransactionAsync_ShouldCommitTransaction()
    {
        await _transactionManagerMock.Object.CommitTransactionAsync();
        _transactionManagerMock.Verify(tm => tm.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task RollbackTransactionAsync_ShouldRollbackTransaction()
    {
        await _transactionManagerMock.Object.RollbackTransactionAsync();
        _transactionManagerMock.Verify(tm => tm.RollbackTransactionAsync(), Times.Once);
    }
}
