using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Mesas;
public class CreateMesaUseCase : ICreateMesaUseCase
{
    private readonly IMesaRepository _mesaRepository;
    private readonly ILogger<CreateMesaUseCase> _logger;
    public CreateMesaUseCase(
        IMesaRepository mesaRepository,
        ILogger<CreateMesaUseCase> logger)
    {
        _mesaRepository = mesaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> ExecuteAsync(MesaDto mesaDto)
    {
        try
        {
            if (await _mesaRepository.GetByNumberAsync(mesaDto.Numero) != null)
                return new OperationResult<Mesa> { Message = "Mesa já registrada", StatusCode = 409 };

            var mesa = new Mesa { Numero = mesaDto.Numero, Status = mesaDto.Status };
            
            await _mesaRepository.AddAsync(mesa);

            return new OperationResult<Mesa> { Result = mesa };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar mesa");
            return new OperationResult<Mesa> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
