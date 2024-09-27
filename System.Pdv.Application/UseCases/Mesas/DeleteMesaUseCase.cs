using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Mesas;

public class DeleteMesaUseCase : IDeleteMesaUseCase
{
    private readonly IMesaRepository _mesaRepository;
    private readonly ILogger<DeleteMesaUseCase> _logger;
    public DeleteMesaUseCase(
        IMesaRepository mesaRepository,
        ILogger<DeleteMesaUseCase> logger
        )
    {
        _mesaRepository = mesaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> ExecuteAsync(Guid id)
    {
        try
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            if (mesa == null)
                return new OperationResult<Mesa> { Message = "Mesa não encontrada", StatusCode = 404 };

            await _mesaRepository.DeleteAsync(mesa);
            return new OperationResult<Mesa> { Result = mesa, Message = "Mesa deletada com sucesso!" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return new OperationResult<Mesa> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }

    }
}
