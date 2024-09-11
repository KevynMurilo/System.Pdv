using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Clientes;

public class GetAllClienteUseCase : IGetAllClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<GetAllClienteUseCase> _logger;

    public GetAllClienteUseCase(IClienteRepository clienteRepository, ILogger<GetAllClienteUseCase> logger)
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Cliente>>> ExecuteAsync(int pageNumber, int pageSize)
    {
        try
        {
            var cliente = await _clienteRepository.GetAllAsync(pageNumber, pageSize);
            if (!cliente.Any()) return new OperationResult<IEnumerable<Cliente>> { Message = "Nenhum cliente encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Cliente>> { Result = cliente };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar clientes");
            return new OperationResult<IEnumerable<Cliente>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
