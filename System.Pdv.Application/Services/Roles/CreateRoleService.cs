using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Roles;

public class CreateRoleService : ICreateRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleService> _logger;

    public CreateRoleService(IRoleRepository roleRepository, ILogger<CreateRoleService> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Role>> CreateRole(RoleDto roleDto)
    {
        try
        {
            var role = new Role
            {
                Nome = roleDto.Nome,
                Descricao = roleDto.Descricao,
            };

            await _roleRepository.AddRoleAsync(role);

            return new OperationResult<Role> { Result = role };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar role");
            return new OperationResult<Role> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
