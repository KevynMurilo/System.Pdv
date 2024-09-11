using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Mesas;

public class GetAllMesaUseCase : IGetAllMesaUseCase
{
    private readonly IMesaRepository _mesaRepository;
    private readonly ILogger<GetAllMesaUseCase> _logger;
    public GetAllMesaUseCase(IMesaRepository mesaRepository, ILogger<GetAllMesaUseCase> logger)
    {
        _mesaRepository = mesaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Mesa>>> ExecuteAsync()
    {
        try
        {
            var mesas = await _mesaRepository.GetAllAsync();
            if (!mesas.Any())
                return new OperationResult<IEnumerable<Mesa>> { Message = "Nenhuma mesa registrada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Mesa>> { Result = mesas };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return new OperationResult<IEnumerable<Mesa>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
