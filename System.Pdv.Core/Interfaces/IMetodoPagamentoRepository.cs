using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IMetodoPagamentoRepository
{
    Task<IEnumerable<MetodoPagamento>> GetAllAsync();
    Task<MetodoPagamento> GetByIdAsync(Guid id);
    Task<MetodoPagamento> GetByNameAsync(string nome);
    Task AddAsync(MetodoPagamento metodoPagamento);
    Task UpdateAsync(MetodoPagamento metodoPagamento);
    Task DeleteAsync(MetodoPagamento metodoPagamento);
}
