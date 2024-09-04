using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Adicionais;

public class GetByIdAdicionalService : IGetByIdAdicionalService
{
    private readonly IAdicionalRepository _adicionalRepository;
    private readonly ILogger<GetByIdAdicionalService> _logger;

    public GetByIdAdicionalService(
        IAdicionalRepository adicionalRepository,
        ILogger<GetByIdAdicionalService> logger)
    {
        _adicionalRepository = adicionalRepository;
        _logger = logger;
    }

    public async Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id)
    {
        try
        {
            var adicional = await _adicionalRepository.GetByIdAsync(id);
            if (adicional == null)
                return new OperationResult<ItemAdicional> { Message = "Adicional não encontrado", StatusCode = 404 };

            return new OperationResult<ItemAdicional> { Result = adicional };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar adicional por id");
            return new OperationResult<ItemAdicional> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
