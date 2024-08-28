using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Garcons;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Garcons;

public class CreateGarcomService : ICreateGarcomService
{
    private readonly IGarcomRepository _garcomRepository;
    private readonly ILogger<CreateGarcomService> _logger;
    public CreateGarcomService(
        IGarcomRepository garcomRepository,
        ILogger<CreateGarcomService> logger)
    {
        _garcomRepository = garcomRepository;
        _logger = logger;
    }   

    public async Task<OperationResult<Garcom>> CreateGarcom(RegisterGarcomDto garcomDto)
    {
        try
        {
            var garcom = new Garcom
            {
                Nome = garcomDto.Nome,
                Email = garcomDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(garcomDto.Password)
            };

            await _garcomRepository.AddAsync(garcom);

            return new OperationResult<Garcom>
            {
                Result = garcom
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar garçom");
            return new OperationResult<Garcom>
            {
                ServerOn = false,
                Message = "Erro inesperado: " + ex.Message,
                StatusCode = 500
            };
        }
    }
}
