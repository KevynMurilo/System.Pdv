using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Security.Claims;

namespace System.Pdv.Application.Services.Pedidos;

public class UpdatePedidoService : IUpdatePedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<UpdatePedidoService> _logger;

    public UpdatePedidoService(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ITransactionManager transactionManager,
        ILogger<UpdatePedidoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedido = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(Guid id, PedidoDto pedidoDto, ClaimsPrincipal userClaims)
    {
        try
        {
            await _transactionManager.BeginTransactionAsync();

            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return new OperationResult<Pedido> { Message = "Pedido não encontrado", StatusCode = 404 };

            var userId = userClaims.FindFirstValue("id");

            var validationResult = await _validarPedidosService.ValidarPedido(pedidoDto, userId);
            if (validationResult != null) return validationResult;

            pedido.Cliente.Nome = pedidoDto.NomeCliente;
            pedido.Cliente.Telefone = pedidoDto.TelefoneCliente;
            pedido.MesaId = pedidoDto.TipoPedido == TipoPedido.Interno ? pedidoDto.MesaId : null;
            pedido.GarcomId = Guid.Parse(userId);
            pedido.MetodoPagamentoId = pedidoDto.MetodoPagamentoId;
            pedido.StatusPedidoId = pedidoDto.StatusPedidoId;

            foreach (var item in pedido.Items.ToList())
            {
                await _pedidoRepository.RemoveItem(item);
            }

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.UpdateAsync(pedido);
            await _transactionManager.CommitTransactionAsync();

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao atualizar o pedido");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

}
