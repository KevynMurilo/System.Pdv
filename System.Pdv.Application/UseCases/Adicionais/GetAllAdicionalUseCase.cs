using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Adicionais;

public class GetAllAdicionalUseCase : IGetAllAdicionalUseCase
{
    private readonly IAdicionalRepository _adicionalRepository;
    private readonly ILogger<GetAllAdicionalUseCase> _logger;

    public GetAllAdicionalUseCase(
        IAdicionalRepository adicionalRepository,
        ILogger<GetAllAdicionalUseCase> logger)
    {
        _adicionalRepository = adicionalRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<ItemAdicional>>> ExecuteAsync(int pageNumber, int pageSize)
    {
        try
        {
            var categorias = await _adicionalRepository.GetAllAsync(pageNumber, pageSize);
            if (!categorias.Any())
                return new OperationResult<IEnumerable<ItemAdicional>> { Message = "Nenhum adicional encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<ItemAdicional>> { Result = categorias };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar adicionais");
            return new OperationResult<IEnumerable<ItemAdicional>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
