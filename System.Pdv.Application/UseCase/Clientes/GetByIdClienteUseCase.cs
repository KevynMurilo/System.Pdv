using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Clientes;

public class GetByIdClienteUseCase : IGetByIdClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<GetByIdClienteUseCase> _logger;

    public GetByIdClienteUseCase(IClienteRepository clienteRepository, ILogger<GetByIdClienteUseCase> logger)
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Cliente>> ExecuteAsync(Guid id)
    {
        try
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return new OperationResult<Cliente> { Message = "Cliente não encontrado", StatusCode = 404 };

            return new OperationResult<Cliente> { Result = cliente };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar cliente por id");
            return new OperationResult<Cliente> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
