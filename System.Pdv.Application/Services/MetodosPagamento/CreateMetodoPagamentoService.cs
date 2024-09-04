using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.MetodosPagamento;

public class CreateMetodoPagamentoService : ICreateMetodoPagamentoService
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<CreateMetodoPagamentoService> _logger;

    public CreateMetodoPagamentoService(
        IMetodoPagamentoRepository metodoPagamentoRepository,
        ILogger<CreateMetodoPagamentoService> logger)
    {
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<MetodoPagamento>> ExecuteAsync(MetodoPagamentoDto metodoPagamentoDto)
    {
        try
        {
            if (await _metodoPagamentoRepository.GetByNameAsync(metodoPagamentoDto.Nome) != null)
                return new OperationResult<MetodoPagamento> { Message = "Método de pagamento já cadastrado", StatusCode = 409 };

            var metodoPagamento = new MetodoPagamento { Nome = metodoPagamentoDto.Nome.ToUpper() };

            await _metodoPagamentoRepository.AddAsync(metodoPagamento);

            return new OperationResult<MetodoPagamento> { Result = metodoPagamento };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar método de pagamento");
            return new OperationResult<MetodoPagamento> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
