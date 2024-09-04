using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Usuarios;

public class UpdateUsuarioService : IUpdateUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdateUsuarioService> _logger;

    public UpdateUsuarioService(
        IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        ILogger<UpdateUsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> ExecuteAsync(Guid id, UsuarioDto usuarioDto)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return new OperationResult<Usuario> { Message = "Usuário não encontrado", StatusCode = 404 };

            if (await _usuarioRepository.GetByEmail(usuarioDto.Email) != null)
                return new OperationResult<Usuario> { Message = "Email já cadastrado", StatusCode = 409 };

            if (await _roleRepository.GetByIdAsync(usuarioDto.RoleId) == null)
            {
                return new OperationResult<Usuario> { Message = "Role não encontrada", StatusCode = 404 };
            }

            usuario.Nome = usuarioDto.Nome;
            usuario.Email = usuarioDto.Email;
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);
            usuario.RoleId = usuarioDto.RoleId;

            await _usuarioRepository.UpdateAsync(usuario);

            return new OperationResult<Usuario> { Result = usuario };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar usuário");
            return new OperationResult<Usuario> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
