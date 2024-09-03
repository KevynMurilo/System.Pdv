using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface IDeleteMetodoPagamentoService
{
    Task<OperationResult<MetodoPagamento>> DeleteMetodoPagamento(Guid id);
}
