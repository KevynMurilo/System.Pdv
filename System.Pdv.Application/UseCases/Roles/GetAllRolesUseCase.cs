﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Roles;

public class GetAllRolesUseCase : IGetAllRolesUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<GetAllRolesUseCase> _logger;

    public GetAllRolesUseCase(
        IRoleRepository roleRepository,
        ILogger<GetAllRolesUseCase> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Role>>> ExecuteAsync()
    {
        try
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            if (!roles.Any()) return new OperationResult<IEnumerable<Role>> {  Message = "Nenhuma role encontrada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Role>>
            {
                Result = roles
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar roles");
            return new OperationResult<IEnumerable<Role>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
