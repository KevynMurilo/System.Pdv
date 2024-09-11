using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface IGetByIdMetodoPagamentoUseCase
{
    Task<OperationResult<MetodoPagamento>> ExecuteAsync(Guid id);
}
