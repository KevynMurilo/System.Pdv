﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Security.Claims;

namespace System.Pdv.Application.UseCase.Pedidos;

public class CreatePedidoUseCase : ICreatePedidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProcessarItensPedidoUseCase _processarItensPedido;
    private readonly IValidarPedidosUseCase _validarPedidosService;
    private readonly ITransactionManager _transactionManager;
    private readonly IThermalPrinterService _thermalPrinterService;
    private readonly ILogger<CreatePedidoUseCase> _logger;

    public CreatePedidoUseCase(
        IPedidoRepository pedidoRepository,
        IProcessarItensPedidoUseCase processarItensPedidoService,
        IValidarPedidosUseCase validarPedidosService,
        ITransactionManager transactionManager,
        IThermalPrinterService thermalPrinterService,
        ILogger<CreatePedidoUseCase> logger)
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
        if (pedidoDto == null || userClaims == null)
            return new OperationResult<Pedido> {Message = "Dados inválidos.", StatusCode = 400 };

        try
        {
            await _transactionManager.BeginTransactionAsync();

            var userId = userClaims.FindFirstValue("id");
            var garcomId = Guid.Parse(userId);

            var validationResult = await _validarPedidosService.ValidarPedido(pedidoDto, userId);
            if (validationResult != null) return validationResult;

            var clienteExistente = await _pedidoRepository.GetClienteByNomeTelefoneAsync(pedidoDto.NomeCliente, pedidoDto.TelefoneCliente);

            var pedido = clienteExistente != null
                ? CreatePedidoComClienteExistente(pedidoDto, clienteExistente.Id, garcomId)
                : CreatePedidoComNovoCliente(pedidoDto, garcomId);

            var processarItensResult = await _processarItensPedido.ExecuteAsync(pedidoDto.Itens, pedido);
            if (processarItensResult != null) return processarItensResult;

            await _pedidoRepository.AddAsync(pedido);
            await _transactionManager.CommitTransactionAsync();

            var printPedido = await _pedidoRepository.GetByIdAsync(pedido.Id);
            var printSuccess = _thermalPrinterService.PrintOrders(new List<Pedido> { printPedido });

            var message = printSuccess
                ? "Pedido criado e impressão realizada com sucesso."
                : "Pedido criado, mas houve um erro na impressão.";

            return new OperationResult<Pedido> { Result = printPedido, Message = message };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido: {Message}", ex.Message);
            return new OperationResult<Pedido> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }


    private Pedido CreatePedidoComClienteExistente(PedidoDto pedidoDto, Guid clienteId, Guid garcomId)
    {
        return new Pedido
        {
            ClienteId = clienteId,
            MesaId = pedidoDto.TipoPedido == TipoPedido.Interno ? pedidoDto.MesaId : null,
            GarcomId = garcomId,
            TipoPedido = pedidoDto.TipoPedido,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId,
            StatusPedidoId = pedidoDto.StatusPedidoId,
            Items = new List<ItemPedido>()
        };
    }

    private Pedido CreatePedidoComNovoCliente(PedidoDto pedidoDto, Guid garcomId)
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