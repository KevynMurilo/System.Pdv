using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Application.Interfaces.Pedidos.Externos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos.Externos;

public class CreatePedidoExternoService : ICreatePedidoExternoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedidoService;
    private readonly ILogger<CreatePedidoExternoService> _logger;

    public CreatePedidoExternoService(
        IPedidoRepository pedidoRepository,
        IUsuarioRepository usuarioRepository,
        IMetodoPagamentoRepository metodoPagamentoRepository,
        IStatusPedidoRepository statusPedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        ILogger<CreatePedidoExternoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _statusPedidoRepository = statusPedidoRepository;
        _processarItensPedidoService = processarItensPedidoService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoExternoDto pedidoExternoDto)
    {
        try
        {
            var validationResult = await ValidarPedidoExternoAsync(pedidoExternoDto);
            if (validationResult != null) return validationResult;

            var pedido = CriarPedidoExterno(pedidoExternoDto);

            var itemResult = await _processarItensPedidoService.ExecuteAsync(pedidoExternoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.AddAsync(pedido);

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido externo");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private async Task<OperationResult<Pedido>> ValidarPedidoExternoAsync(PedidoExternoDto pedidoExternoDto)
    {
        if (await _usuarioRepository.GetByIdAsync(pedidoExternoDto.GarcomId) == null)
            return new OperationResult<Pedido> { Message = "Garçom não encontrado", StatusCode = 404 };

        if (await _metodoPagamentoRepository.GetByIdAsync(pedidoExternoDto.MetodoPagamentoId) == null)
            return new OperationResult<Pedido> { Message = "Método de pagamento inválido", StatusCode = 400 };

        if (await _statusPedidoRepository.GetByIdAsync(pedidoExternoDto.StatusPedidoId) == null)
            return new OperationResult<Pedido> { Message = "Status de pedido inválido", StatusCode = 400 };

        return null;
    }

    private Pedido CriarPedidoExterno(PedidoExternoDto pedidoDto)
    {
        return new Pedido
        {
            ClienteId = pedidoDto.ClienteId,
            GarcomId = pedidoDto.GarcomId,
            TipoPedido = TipoPedido.Externo,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }
}