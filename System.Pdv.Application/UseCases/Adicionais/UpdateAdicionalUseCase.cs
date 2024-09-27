using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Adicionais;

public class UpdateAdicionalUseCase : IUpdateAdicionalUseCase
{
    private readonly IAdicionalRepository _adicionalRepository;
    private readonly ILogger<UpdateAdicionalUseCase> _logger;

    public UpdateAdicionalUseCase(
        IAdicionalRepository adicionalRepository,
        ILogger<UpdateAdicionalUseCase> logger)
    {
        _adicionalRepository = adicionalRepository;
        _logger = logger;
    }

    public async Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id, UpdateAdicionalDto adicionalDto)
    {
        try
        {
            var adicional = await _adicionalRepository.GetByIdAsync(id);
            if (adicional == null)
                return new OperationResult<ItemAdicional> { Message = "Adicional não encontrado", StatusCode = 404 };

            UpdateFromDto(adicional, adicionalDto);

            await _adicionalRepository.UpdateAsync(adicional);

            return new OperationResult<ItemAdicional> { Result = adicional, Message = "Adicional atualizado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar adicional");
            return new OperationResult<ItemAdicional> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private void UpdateFromDto(ItemAdicional target, UpdateAdicionalDto source)
    {
        if (!string.IsNullOrEmpty(source.Nome))
            target.Nome = source.Nome.ToUpper();

        if (source.Preco.HasValue)
            target.Preco = source.Preco.Value;
    }
}
