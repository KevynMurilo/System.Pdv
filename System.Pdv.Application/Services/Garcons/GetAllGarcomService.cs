using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Garcons;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Garcons;

public class GetAllGarcomService : IGetAllGarcomService
{
    private readonly IGarcomRepository _garcomRepository;
    private readonly ILogger<GetAllGarcomService> _logger;
    public GetAllGarcomService(
        IGarcomRepository garcomRepository,
        ILogger<GetAllGarcomService> logger)
    {
        _garcomRepository = garcomRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Garcom>>> GetAllGarcom()
    {
        try
        {
            var garcom = await _garcomRepository.GetAllAsync();
            if (garcom == null || !garcom.Any())
            {
                return new OperationResult<IEnumerable<Garcom>>
                {
                    Message = "Nenhum garçcom encontrado",
                    StatusCode = 404
                };
            }

            return new OperationResult<IEnumerable<Garcom>>
            {
                Result = garcom
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao buscar garçons");
            return new OperationResult<IEnumerable<Garcom>>
            {
                ServerOn = false,
                Message = "Erro inesperado: " + ex.Message,
                StatusCode = 500
            };
        }
    }
}
