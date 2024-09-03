using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.MetodosPagamento;

public class GetAllMetodoPagamentoServices : IGetAllMetodoPagamentoServices
{
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly ILogger<GetAllMetodoPagamentoServices> _logger;

    public GetAllMetodoPagamentoServices(IMetodoPagamentoRepository metodoPagamentoRepository, ILogger<GetAllMetodoPagamentoServices> logger)
    {
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<MetodoPagamento>>> GetAllMetodoPagamento()
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
            return new OperationResult<IEnumerable<MetodoPagamento>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
