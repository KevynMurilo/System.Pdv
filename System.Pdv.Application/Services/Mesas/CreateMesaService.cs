﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.MesaService;

public class CreateMesaService : ICreateMesaService
{
    private readonly IMesaRepository _mesaRepository;
    private readonly ILogger<CreateMesaService> _logger;
    public CreateMesaService(
        IMesaRepository mesaRepository,
        ILogger<CreateMesaService> logger)
    {
        _mesaRepository = mesaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Mesa>> CreateMesa(MesaDto mesaDto)
    {
        try
        {
            var mesaExists = await _mesaRepository.GetByNumberAsync(mesaDto.Numero);

            if (mesaExists != null)
            {
                return new OperationResult<Mesa>
                {
                    Message = "Mesa já registrada",
                    StatusCode = 409
                };
            }

            var mesa = new Mesa
            {
                Numero = mesaDto.Numero,
            };

            await _mesaRepository.AddAsync(mesa);

            return new OperationResult<Mesa>
            {
                Result = mesa
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar mesa");
            return new OperationResult<Mesa>
            {
                Status = false,
                Message = "Erro inesperado: " + ex.Message,
                StatusCode = 500
            };
        }
    }
}
