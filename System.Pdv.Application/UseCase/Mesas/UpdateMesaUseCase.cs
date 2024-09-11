using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Mesas;

public class UpdateMesaUseCase: IUpdateMesaUseCase
{
    private readonly IMesaRepository _mesaRepository;
    private readonly ILogger<UpdateMesaUseCase> _logger;
    public UpdateMesaUseCase(
        IMesaRepository mesaRepository,
        ILogger<UpdateMesaUseCase> logger)
    {
        _mesaRepository = mesaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> ExecuteAsync(Guid id, MesaDto mesaDto)
    {
        try
        {
            var mesa = await _mesaRepository.GetByIdAsync(id);
            if (mesa == null)
                return new OperationResult<Mesa> { Message = "Mesa não encontrada", StatusCode = 404 };

            mesa.Numero = mesaDto.Numero;
            mesa.Status = mesaDto.Status;

            await _mesaRepository.UpdateAsync(mesa);

            return new OperationResult<Mesa> { Result = mesa };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar mesa");
            return new OperationResult<Mesa> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
