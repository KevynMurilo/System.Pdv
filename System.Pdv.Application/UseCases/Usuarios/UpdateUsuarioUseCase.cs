using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Usuarios;

public class UpdateUsuarioUseCase : IUpdateUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdateUsuarioUseCase> _logger;

    public UpdateUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        ILogger<UpdateUsuarioUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> ExecuteAsync(Guid id, UpdateUsuarioDto usuarioDto)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return new OperationResult<Usuario> { Message = "Usuário não encontrado", StatusCode = 404 };

            if (!string.IsNullOrEmpty(usuarioDto.Email) && await _usuarioRepository.GetByEmail(usuarioDto.Email) != null)
                return new OperationResult<Usuario> { Message = "Email já cadastrado", StatusCode = 409 };

            if (usuarioDto.RoleId != Guid.Empty && await _roleRepository.GetByIdAsync(usuarioDto.RoleId) == null)
                return new OperationResult<Usuario> { Message = "Role não encontrada", StatusCode = 404 };

            UpdateFromDto(usuario, usuarioDto);

            await _usuarioRepository.UpdateAsync(usuario);

            return new OperationResult<Usuario> { Result = usuario };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar usuário");
            return new OperationResult<Usuario> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private void UpdateFromDto(Usuario target, UpdateUsuarioDto source)
    {
        if (!string.IsNullOrEmpty(source.Nome))
            target.Nome = source.Nome;

        if (!string.IsNullOrEmpty(source.Email))
            target.Email = source.Email;

        if (!string.IsNullOrEmpty(source.Password))
            target.PasswordHash = BCrypt.Net.BCrypt.HashPassword(source.Password);

        if (source.RoleId != Guid.Empty)
            target.RoleId = source.RoleId;
    }
}
