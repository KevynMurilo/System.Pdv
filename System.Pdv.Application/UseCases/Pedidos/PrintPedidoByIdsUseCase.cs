using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class PrintPedidoByIdsUseCase : IPrintPedidoByIdsUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IThermalPrinterService _thermalPrinterService;
    private readonly ILogger<PrintPedidoByIdsUseCase> _logger;

    public PrintPedidoByIdsUseCase(
        IPedidoRepository pedidoRepository,
        IThermalPrinterService thermalPrinterService,
        ILogger<PrintPedidoByIdsUseCase> logger)
    {
        _pedidoRepository = pedidoRepository;
        _thermalPrinterService = thermalPrinterService;
        _logger = logger;
    }

    public async Task<OperationResult<List<Pedido>>> ExecuteAsync(IEnumerable<Guid> ids)
    {
        try
        {
            var pedidos = await _pedidoRepository.GetPedidosByIdsAsync(ids);
            if (pedidos == null || !pedidos.Any())
                return new OperationResult<List<Pedido>> { Message = "Nenhum pedido encontrado", StatusCode = 404 };

            var print = _thermalPrinterService.PrintOrders(pedidos);
            var message = print
                ? "Pedidos impressos com sucesso"
                : "Ocorreu um erro ao imprimir os pedidos";

            return new OperationResult<List<Pedido>> { Result = pedidos, Message = message };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao imprimir pedido por IDs");
            return new OperationResult<List<Pedido>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
