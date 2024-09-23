using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.MetodosPagamento;
public class DeleteMetodoPagamentoUseCase : IDeleteMetodoPagamentoUseCase
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<DeleteMetodoPagamentoUseCase> _logger;

    public DeleteMetodoPagamentoUseCase(
        IMetodoPagamentoRepository metodoPagamentoRepository,
        ILogger<DeleteMetodoPagamentoUseCase> logger)
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

            await _metodoPagamentoRepository.DeleteAsync(metodoPagamento);

            return new OperationResult<MetodoPagamento> { Result = metodoPagamento, Message = "Método de pagamento deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar método de pagamento");
            return new OperationResult<MetodoPagamento> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
