using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.MetodosPagamento;
public class GetAllMetodoPagamentoUseCase : IGetAllMetodoPagamentoUseCase
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<GetAllMetodoPagamentoUseCase> _logger;

    public GetAllMetodoPagamentoUseCase(
        IMetodoPagamentoRepository metodoPagamentoRepository,
        ILogger<GetAllMetodoPagamentoUseCase> logger)
    {
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<MetodoPagamento>>> ExecuteAsync()
    {
        try
        {
            var metodoPagamento = await _metodoPagamentoRepository.GetAllAsync();
            if (!metodoPagamento.Any())
                return new OperationResult<IEnumerable<MetodoPagamento>> { Message = "Nenhum método de pagamento encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<MetodoPagamento>> { Result = metodoPagamento };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar métodos de pagamento");
            return new OperationResult<IEnumerable<MetodoPagamento>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
