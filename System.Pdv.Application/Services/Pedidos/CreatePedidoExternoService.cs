using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class CreatePedidoExternoService : ICreatePedidoExternoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedidoService;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ILogger<CreatePedidoExternoService> _logger;

    public CreatePedidoExternoService(
        IPedidoRepository pedidoRepository,
        IUsuarioRepository usuarioRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ILogger<CreatePedidoExternoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedidoService = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(PedidoExternoDto pedidoExternoDto)
    {
        try
        {
            var validationResult = await _validarPedidosService.ValidarPedidoExternoAsync(pedidoExternoDto);
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

    private Pedido CriarPedidoExterno(PedidoExternoDto pedidoDto)
    {
        return new Pedido
        {
            Cliente = new Cliente
            {
                Nome = pedidoDto.NomeCliente,
                Telefone = pedidoDto.TelefoneCliente,
            },
            GarcomId = pedidoDto.GarcomId,
            TipoPedido = TipoPedido.Externo,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }
}