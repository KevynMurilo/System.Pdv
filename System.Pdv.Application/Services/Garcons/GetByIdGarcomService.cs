using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Garcons;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Garcons;

public class GetByIdGarcomService : IGetByIdGarcomService
{
    private readonly IGarcomRepository _garcomRepository;
    private readonly ILogger<GetByIdGarcomService> _logger;

    public GetByIdGarcomService(IGarcomRepository garcomRepository, ILogger<GetByIdGarcomService> logger)
    {
        _garcomRepository = garcomRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Garcom>> GetById(Guid id)
    {
        try
        {
            var garcom = await _garcomRepository.GetByIdAsync(id);
            if (garcom == null)
            {
                return new OperationResult<Garcom>
                {
                    Message = "Garçom não encontrado",
                    StatusCode = 404
                };
            }

            return new OperationResult<Garcom>
            {
                Result = garcom,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar garçom por id");
            return new OperationResult<Garcom>
            {
                ServerOn = false,
                Message = "Erro inesperado: " + ex.Message,
                StatusCode = 500
            };
        }
    }
}
