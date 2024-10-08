﻿using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class ProcessarItensPedidoUseCase : IProcessarItensPedidoUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IAdicionalRepository _adicionalRepository;
    public ProcessarItensPedidoUseCase(
        IProdutoRepository produtoRepository,
        IAdicionalRepository adicionalRepository)
    {
        _produtoRepository = produtoRepository;
        _adicionalRepository = adicionalRepository;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(IList<ItemPedidoDto> itensDto, Pedido pedido)
    {
        foreach (var itemDto in itensDto)
        {
            var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
            if (produto == null)
                return new OperationResult<Pedido> { Message = $"Produto com ID {itemDto.ProdutoId} não encontrado", StatusCode = 404 };

            IList<ItemAdicional> adicionais = new List<ItemAdicional>();
            if (itemDto.AdicionalId != null && itemDto.AdicionalId.Any())
            {
                adicionais = await _adicionalRepository.GetAdicionaisByIdsAsync(itemDto.AdicionalId);

                var notFoundIds = itemDto.AdicionalId.Except(adicionais.Select(a => a.Id)).ToList();
                if (notFoundIds.Any())
                    return new OperationResult<Pedido> { Message = $"Os adicionais com IDs {string.Join(", ", notFoundIds)} não foram encontrados", StatusCode = 404 };
            }
            
            var itemPedido = new ItemPedido
            {
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                Observacoes = itemDto.Observacoes,
                Adicionais = adicionais,
            };

            pedido.Items.Add(itemPedido);
        }

        return null;
    }

}
