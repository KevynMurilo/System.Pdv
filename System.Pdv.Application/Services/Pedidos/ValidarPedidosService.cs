using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class ValidarPedidosService : IValidarPedidosService
{
    private readonly IMesaRepository _mesaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedido;

    public ValidarPedidosService(
        IMesaRepository mesaRepository,
        IUsuarioRepository usuarioRepository,
        IMetodoPagamentoRepository metodoPagamentoRepository,
        IStatusPedidoRepository statusPedidoRepository)
    {
        _mesaRepository = mesaRepository;
        _usuarioRepository = usuarioRepository;
        _metodoPagamentoRepository = metodoPagamentoRepository;
        _statusPedidoRepository = statusPedidoRepository;
    }

    public async Task<OperationResult<Pedido>> ValidarPedidoInternoAsync(PedidoInternoDto pedidoDto)
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

    public async Task<OperationResult<Pedido>> ValidarPedidoExternoAsync(PedidoExternoDto pedidoExternoDto)
    {
        if (await _usuarioRepository.GetByIdAsync(pedidoExternoDto.GarcomId) == null)
            return new OperationResult<Pedido> { Message = "Garçom não encontrado", StatusCode = 404 };

        if (await _metodoPagamentoRepository.GetByIdAsync(pedidoExternoDto.MetodoPagamentoId) == null)
            return new OperationResult<Pedido> { Message = "Método de pagamento inválido", StatusCode = 400 };

        if (await _statusPedidoRepository.GetByIdAsync(pedidoExternoDto.StatusPedidoId) == null)
            return new OperationResult<Pedido> { Message = "Status de pedido inválido", StatusCode = 400 };

        return null;
    }
}
