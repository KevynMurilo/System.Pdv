using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Security.Claims;

namespace System.Pdv.Application.UseCase.Pedidos;

public class UpdatePedidoUseCase : IUpdatePedidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoUseCase _processarItensPedido;
    private readonly IValidarPedidosUseCase _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<UpdatePedidoUseCase> _logger;

    public UpdatePedidoUseCase(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoUseCase processarItensPedidoService,
        IValidarPedidosUseCase validarPedidosService,
        ITransactionManager transactionManager,
        ILogger<UpdatePedidoUseCase> logger)
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

            var clienteExistente = await _pedidoRepository.GetClienteByNomeTelefoneAsync(pedidoDto.NomeCliente, pedidoDto.TelefoneCliente);

            if (clienteExistente != null)
            {
                // Se o cliente já existe, atualiza o pedido com o ID do cliente existente
                pedido.ClienteId = clienteExistente.Id;
            }
            else
            {
                // Se o cliente não existe, cria um novo cliente
                pedido.Cliente = new Cliente
                {
                    Nome = pedidoDto.NomeCliente,
                    Telefone = pedidoDto.TelefoneCliente,
                };
            }

            pedido.MesaId = pedidoDto.TipoPedido == TipoPedido.Interno ? pedidoDto.MesaId : null;
            pedido.GarcomId = Guid.Parse(userId);
            pedido.MetodoPagamentoId = pedidoDto.MetodoPagamentoId;
            pedido.TipoPedido = pedidoDto.TipoPedido;
            pedido.StatusPedidoId = pedidoDto.StatusPedidoId;

            foreach (var item in pedido.Items.ToList())
            {
                await _pedidoRepository.RemoveItem(item);
            }

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.UpdateAsync(pedido);
            await _transactionManager.CommitTransactionAsync();

            return new OperationResult<Pedido> { Result = pedido, Message = "Pedido atualizado com sucesso" };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao atualizar o pedido");
            return new OperationResult<Pedido> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
