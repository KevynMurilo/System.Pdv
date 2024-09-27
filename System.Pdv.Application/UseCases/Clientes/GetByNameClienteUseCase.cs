using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Clientes;

public class GetByNameClienteUseCase : IGetByNameClienteUseCase
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<GetByNameClienteUseCase> _logger;

    public GetByNameClienteUseCase(IClienteRepository clienteRepository, ILogger<GetByNameClienteUseCase> logger)
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Cliente>>> ExecuteAsync(string nome)
    {
        try
        {
            var clientes = await _clienteRepository.GetByNameAsync(nome);
            if (!clientes.Any()) return new OperationResult<IEnumerable<Cliente>> { Message = "Nenhum cliente com esse nome encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Cliente>> { Result = clientes };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar clientes com nome especifico");
            return new OperationResult<IEnumerable<Cliente>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
