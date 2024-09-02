using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Mesas;

public class GetMesaByIdService : IGetMesaByIdService
{
    private readonly IMesaRepository _repository;
    private readonly ILogger<GetMesaByIdService> _logger;
    public GetMesaByIdService(
        IMesaRepository repository,
        ILogger<GetMesaByIdService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> GetById(Guid id)
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
            return new OperationResult<Mesa> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
