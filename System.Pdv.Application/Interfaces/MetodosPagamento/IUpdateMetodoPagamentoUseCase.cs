using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface IUpdateMetodoPagamentoUseCase
{
    Task<OperationResult<MetodoPagamento>> ExecuteAsync(Guid id, MetodoPagamentoDto metodoPagamentoDto);

}
