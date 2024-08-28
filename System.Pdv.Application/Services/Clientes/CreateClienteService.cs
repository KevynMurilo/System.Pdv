using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Clientes;

public class CreateClienteService : ICreateClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<CreateClienteService> _logger;

    public CreateClienteService(
        IClienteRepository clienteRepository,
        ILogger<CreateClienteService> logger
        )
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Cliente>> CreateCliente(ClienteDto clienteDto)
    {
        try
        {
            var cliente = new Cliente
            {
                Nome = clienteDto.Nome,
                Telefone = clienteDto.Telefone,
            };

            await _clienteRepository.AddAsync(cliente);

            return new OperationResult<Cliente>
            {
                Result = cliente,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar cliente");
            return new OperationResult<Cliente>
            {
                ServerOn = false,
                Message = "Erro inesperado: " + ex.Message,
                StatusCode = 500
            };
        }
    }
}
