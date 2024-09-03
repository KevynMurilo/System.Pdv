using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Adicionais;

public class CreateAdicionalService : ICreateAdicionalService
{
    private readonly IAdicionalRepository _adicionalRepository;
    private readonly ILogger<CreateAdicionalService> _logger;

    public CreateAdicionalService(IAdicionalRepository adicionalRepository, ILogger<CreateAdicionalService> logger)
    {
        _adicionalRepository = adicionalRepository;
        _logger = logger;
    }

    public async Task<OperationResult<ItemAdicional>> CreateAdicional(AdicionalDto adicionalDto)
    {
        try
        {
            if (await _adicionalRepository.GetByNameAsync(adicionalDto.Nome) != null)
                return new OperationResult<ItemAdicional> { Message = "Adicional já registrado", StatusCode = 409 };

            var adicional = new ItemAdicional { Nome = adicionalDto.Nome.ToUpper(), Preco = adicionalDto.Preco };

            await _adicionalRepository.AddAsync(adicional);

            return new OperationResult<ItemAdicional> { Result = adicional };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar adicional");
            return new OperationResult<ItemAdicional> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
