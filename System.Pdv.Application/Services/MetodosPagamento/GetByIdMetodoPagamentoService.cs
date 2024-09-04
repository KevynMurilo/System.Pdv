using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.MetodosPagamento;

public class GetByIdMetodoPagamentoService : IGetByIdMetodoPagamentoService
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<GetByIdMetodoPagamentoService> _logger;

    public GetByIdMetodoPagamentoService(
        IMetodoPagamentoRepository metodoPagamentoRepository,
        ILogger<GetByIdMetodoPagamentoService> logger)
    {
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<MetodoPagamento>> ExecuteAsync(Guid id)
    {
        try
        {
            var metodoPagamento = await _metodoPagamentoRepository.GetByIdAsync(id);
            if (metodoPagamento == null)
                return new OperationResult<MetodoPagamento> { Message = "Método de pagamento não encontrado", StatusCode = 404 };

            return new OperationResult<MetodoPagamento> { Result = metodoPagamento };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar método de pagamento");
            return new OperationResult<MetodoPagamento> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
