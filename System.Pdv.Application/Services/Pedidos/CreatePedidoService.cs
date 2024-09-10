using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Security.Claims;

namespace System.Pdv.Application.Services.Pedidos;

public class CreatePedidoService : ICreatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly IThermalPrinterService _thermalPrinterService;
    private readonly ILogger<CreatePedidoService> _logger;

    public CreatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ITransactionManager transactionManager,
        IThermalPrinterService thermalPrinterService,
        ILogger<CreatePedidoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedido = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _transactionManager = transactionManager;
        _thermalPrinterService = thermalPrinterService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoDto pedidoDto, ClaimsPrincipal userClaims)
    {
        try
        {
            await _transactionManager.BeginTransactionAsync();

            var userId = userClaims.FindFirstValue("id");

            var validationResult = await _validarPedidosService.ValidarPedido(pedidoDto, userId);
            if (validationResult != null) return validationResult;

            var pedido = CreatePedido(pedidoDto, Guid.Parse(userId));

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.AddAsync(pedido);
            await _transactionManager.CommitTransactionAsync();

            var returnPedido = await _pedidoRepository.GetByIdAsync(pedido.Id);

            var printSuccess = _thermalPrinterService.PrintOrder(returnPedido);
            var message = printSuccess == true
                ? "Pedido criado e impressão realizada com sucesso."
                : "Pedido criado, mas houve um erro na impressão.";

            return new OperationResult<Pedido> { Result = returnPedido, Message = message };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private Pedido CreatePedido(PedidoDto pedidoDto, Guid garcomId)
    {
        return new Pedido
        {
            Cliente = new Cliente
            {
                Nome = pedidoDto.NomeCliente,
                Telefone = pedidoDto.TelefoneCliente,
            },
            MesaId = pedidoDto.TipoPedido == TipoPedido.Interno ? pedidoDto.MesaId : null,
            GarcomId = garcomId,
            TipoPedido = pedidoDto.TipoPedido,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }
}