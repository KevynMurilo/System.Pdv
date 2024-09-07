using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class CreatePedidoInternoService : ICreatePedidoInternoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<CreatePedidoInternoService> _logger;

    public CreatePedidoInternoService(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ITransactionManager transactionManager,
        ILogger<CreatePedidoInternoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedido = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoInternoDto pedidoDto)
    {
        try
        {
            await _transactionManager.BeginTransactionAsync();
            var validationResult = await _validarPedidosService.ValidarPedidoInternoAsync(pedidoDto);
            if (validationResult != null) return validationResult;

            var pedido = CriarPedidoInterno(pedidoDto);

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.AddAsync(pedido);
            await _transactionManager.CommitTransactionAsync(); 

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido interno");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private Pedido CriarPedidoInterno(PedidoInternoDto pedidoDto)
    {
        return new Pedido
        {
            MesaId = pedidoDto.MesaId,
            GarcomId = pedidoDto.GarcomId,
            TipoPedido = TipoPedido.Interno,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }
}
