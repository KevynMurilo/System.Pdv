using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Mesas;

public class GetMesaByIdUseCase : IGetMesaByIdUseCase
{
    private readonly IMesaRepository _repository;
    private readonly ILogger<GetMesaByIdUseCase> _logger;
    public GetMesaByIdUseCase(
        IMesaRepository repository,
        ILogger<GetMesaByIdUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> ExecuteAsync(Guid id)
    {
        try
        {
            var mesa = await _repository.GetByIdAsync(id);
            if (mesa == null)
                return new OperationResult<Mesa> { Message = "Mesa não encontrada", StatusCode = 404 };

            return new OperationResult<Mesa> { Result = mesa };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesa por id");
            return new OperationResult<Mesa> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
