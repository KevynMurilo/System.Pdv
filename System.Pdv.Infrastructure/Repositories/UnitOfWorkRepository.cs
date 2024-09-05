using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWorkRepository(AppDbContext context, IDbContextTransaction transaction)
    {
        _context = context;
        _transaction = transaction;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }

}
