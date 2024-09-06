using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;


namespace System.Pdv.Application.Services.Pedidos;

public class UpdatePedidoInternoService : IUpdatePedidoInternoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoService _processarItensPedidoService;
    private readonly IValidarPedidosService _validarPedidosService;
    private readonly ILogger<UpdatePedidoInternoService> _logger;

    public UpdatePedidoInternoService(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoService processarItensPedidoService,
        IValidarPedidosService validarPedidosService,
        ILogger<UpdatePedidoInternoService> logger)
    {
        _pedidoRepository = pedidoRepository;
        _processarItensPedidoService = processarItensPedidoService;
        _validarPedidosService = validarPedidosService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(Guid id, PedidoInternoDto pedidoDto)
    {
        try
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return new OperationResult<Pedido> { Message = "Pedido não encontrado", StatusCode = 404 };

            var validationResult = await _validarPedidosService.ValidarPedidoInternoAsync(pedidoDto);
            if (validationResult != null) return validationResult;

            pedido.MesaId = pedidoDto.MesaId;
            pedido.GarcomId = pedidoDto.GarcomId;
            pedido.MetodoPagamentoId = pedidoDto.MetodoPagamentoId;
            pedido.StatusPedidoId = pedidoDto.StatusPedidoId;

            await AtualizarItensPedido(pedido, pedidoDto.Itens);

            await _pedidoRepository.UpdateAsync(pedido);

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar pedido interno");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private async Task AtualizarItensPedido(Pedido pedido, IList<ItemPedidoDto> itensDto)
    {
        var itensARemover = pedido.Items.Where(i => !itensDto.Any(d => d.ProdutoId == i.ProdutoId)).ToList();
        foreach (var item in itensARemover)
        {
            pedido.Items.Remove(item);
        }

        foreach (var itemDto in itensDto)
        {
            var itemExistente = pedido.Items.FirstOrDefault(i => i.ProdutoId == itemDto.ProdutoId);
            if (itemExistente != null)
            {
                itemExistente.Quantidade = itemDto.Quantidade;
                itemExistente.Observacoes = itemDto.Observacoes;
            }
            else
            {
                var novoItem = new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    Observacoes = itemDto.Observacoes
                };
                pedido.Items.Add(novoItem);
            }
        }
    }
}
