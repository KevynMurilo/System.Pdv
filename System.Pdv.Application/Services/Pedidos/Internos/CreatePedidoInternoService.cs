using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Application.Interfaces.Pedidos.Internos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class CreatePedidoInternoService : ICreatePedidoInternoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMesaRepository _mesaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;
    private readonly ILogger<CreatePedidoInternoService> _logger;

    public CreatePedidoInternoService(
        IPedidoRepository pedidoRepository,
        IMesaRepository mesaRepository,
        IUsuarioRepository usuarioRepository,
        IMetodoPagamentoRepository metodoPagamentoRepository,
        IStatusPedidoRepository statusPedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        ILogger<CreatePedidoInternoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _mesaRepository = mesaRepository;
        _usuarioRepository = usuarioRepository;
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _statusPedidoRepository = statusPedidoRepository;
        _processarItensPedido = processarItensPedidoService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoInternoDto pedidoDto)
    {
        try
        {
            var validationResult = await ValidarPedidoInternoAsync(pedidoDto);
            if (validationResult != null) return validationResult;

            var pedido = CriarPedidoInterno(pedidoDto);

            var itemResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (itemResult != null) return itemResult;

            await _pedidoRepository.AddAsync(pedido);

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido interno");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private async Task<OperationResult<Pedido>> ValidarPedidoInternoAsync(PedidoInternoDto pedidoDto)
    {
        if (await _mesaRepository.GetByIdAsync(pedidoDto.MesaId) == null)
            return new OperationResult<Pedido> { Message = "Mesa não encontrada", StatusCode = 404 };

        if (await _usuarioRepository.GetByIdAsync(pedidoDto.GarcomId) == null)
            return new OperationResult<Pedido> { Message = "Garçom não encontrado", StatusCode = 404 };

        if (await _metodoPagamentoRepository.GetByIdAsync(pedidoDto.MetodoPagamentoId) == null)
            return new OperationResult<Pedido> { Message = "Método de pagamento inválido", StatusCode = 400 };

        if (await _statusPedidoRepository.GetByIdAsync(pedidoDto.StatusPedidoId) == null)
            return new OperationResult<Pedido> { Message = "Status de pedido inválido", StatusCode = 400 };

        return null;
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
