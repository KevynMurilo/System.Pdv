using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.MetodosPagamento;

public class UpdateMetodoPagamentoUseCase : IUpdateMetodoPagamentoUseCase
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<UpdateMetodoPagamentoUseCase> _logger;

    public UpdateMetodoPagamentoUseCase(
        IMetodoPagamentoRepository metodoPagamentoRepository,
        ILogger<UpdateMetodoPagamentoUseCase> logger)
    {
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<MetodoPagamento>> ExecuteAsync(Guid id, MetodoPagamentoDto metodoPagamentoDto)
    {
        try
        {
            var metodoPagamento = await _metodoPagamentoRepository.GetByIdAsync(id);
            if (metodoPagamento == null)
                return new OperationResult<MetodoPagamento> { Message = "Método de pagamento não encontrado", StatusCode = 404 };

            if (await _metodoPagamentoRepository.GetByNameAsync(metodoPagamentoDto.Nome) != null)
                return new OperationResult<MetodoPagamento> { Message = "Método de pagamento já cadastrado", StatusCode = 409 };

            metodoPagamento.Nome = metodoPagamentoDto.Nome.ToUpper();

            await _metodoPagamentoRepository.UpdateAsync(metodoPagamento);

            return new OperationResult<MetodoPagamento> { Result = metodoPagamento };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar método de pagamento");
            return new OperationResult<MetodoPagamento> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
