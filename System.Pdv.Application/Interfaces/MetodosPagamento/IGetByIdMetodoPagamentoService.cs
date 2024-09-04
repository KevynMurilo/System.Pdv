using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface IGetByIdMetodoPagamentoService
{
    Task<OperationResult<MetodoPagamento>> ExecuteAsync(Guid id);
}
