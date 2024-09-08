using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class CreatePedidoService : ICreatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<CreatePedidoService> _logger;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ITransactionManager transactionManager,
        ILogger<CreatePedidoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedido = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoDto pedidoDto)
    {
        try
        {
            await _transactionManager.BeginTransactionAsync();

            var validationResult = await _validarPedidosService.ValidarPedido(pedidoDto);
            if (validationResult != null) return validationResult;

            var pedido = CreatePedido(pedidoDto);

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.AddAsync(pedido);
            await _transactionManager.CommitTransactionAsync(); 

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private Pedido CreatePedido(PedidoDto pedidoDto)
    {
        return new Pedido
        {
            Cliente = new Cliente
            {
                Nome = pedidoDto.NomeCliente,
                Telefone = pedidoDto.TelefoneCliente,
            },
            MesaId = pedidoDto.TipoPedido == TipoPedido.Interno ? pedidoDto.MesaId : null,
            GarcomId = pedidoDto.GarcomId,
            TipoPedido = pedidoDto.TipoPedido,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }
}
