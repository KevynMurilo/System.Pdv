using Microsoft.IdentityModel.Tokens;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class ValidarPedidosUseCase : IValidarPedidosUseCase
{
    private readonly IMesaRepository _mesaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly IProcessarItensPedidoUseCase _processarItensPedido;

    public ValidarPedidosUseCase(
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

    public async Task<OperationResult<Pedido>> ValidarPedido(PedidoDto pedidoDto, string garcomId)
    {
        if (pedidoDto.TipoPedido == TipoPedido.Interno)
        {
            if (pedidoDto.MesaId == Guid.Empty)
                return new OperationResult<Pedido> { Message = "O ID da mesa é obrigatório para pedidos internos.", StatusCode = 400 };

            if (await _mesaRepository.GetByIdAsync(pedidoDto.MesaId) == null)
                return new OperationResult<Pedido> { Message = "Mesa não encontrada ou inválida", StatusCode = 404 };
        }

        if (pedidoDto.TipoPedido == TipoPedido.Externo)
        {
            if (string.IsNullOrEmpty(pedidoDto.NomeCliente) || string.IsNullOrEmpty(pedidoDto.TelefoneCliente))
                return new OperationResult<Pedido> { Message = "Nome e telefone do cliente são obrigatórios para pedidos externos.", StatusCode = 400 };

            if (pedidoDto.MesaId != Guid.Empty)
                return new OperationResult<Pedido> { Message = "O ID da mesa não deve ser informado para pedidos externos.", StatusCode = 400 };
        }

        if (string.IsNullOrEmpty(garcomId))
            return new OperationResult<Pedido> { Message = "Token inválido. ID do garçom não encontrado.", StatusCode = 401 };

        if (await _metodoPagamentoRepository.GetByIdAsync(pedidoDto.MetodoPagamentoId) == null)
            return new OperationResult<Pedido> { Message = "Método de pagamento inválido", StatusCode = 400 };

        if (await _statusPedidoRepository.GetByIdAsync(pedidoDto.StatusPedidoId) == null)
            return new OperationResult<Pedido> { Message = "Status de pedido inválido", StatusCode = 400 };

        return null;
    }
}