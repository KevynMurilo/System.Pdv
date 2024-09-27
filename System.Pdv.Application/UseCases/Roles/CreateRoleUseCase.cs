using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Roles;

public class CreateRoleUseCase : ICreateRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleUseCase> _logger; 

    public CreateRoleUseCase(IRoleRepository roleRepository, ILogger<CreateRoleUseCase> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Role>> ExecuteAsync(CreateRoleDto roleDto)
    {
        try
        {
            var roleExists = await _roleRepository.GetByNameAsync(roleDto.Nome);
            if (roleExists != null) return new OperationResult<Role> { Message = "Role já cadastrada", StatusCode = 409 };

            var role = new Role
            {
                Nome = roleDto.Nome.ToUpper(),
                Descricao = roleDto.Descricao,
            };

            await _roleRepository.AddAsync(role);

            return new OperationResult<Role> { Result = role };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar role");
            return new OperationResult<Role> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
