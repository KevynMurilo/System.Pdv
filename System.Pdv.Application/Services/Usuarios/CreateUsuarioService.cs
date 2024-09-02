using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Usuarios;
public class CreateUsuarioService : ICreateUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateUsuarioService> _logger;
    public CreateUsuarioService(
        IUsuarioRepository usuarioRepository,
        IRoleRepository roleRepository,
        ILogger<CreateUsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }   

    public async Task<OperationResult<Usuario>> CreateUsuario(UsuarioDto usuarioDto)
    {
        try
        {
            if (await _usuarioRepository.GetByEmail(usuarioDto.Email) != null)
            {
                return new OperationResult<Usuario> { Message = "Email já cadastrado", StatusCode = 409 };
            }

            if (await _roleRepository.GetByIdAsync(usuarioDto.RoleId) == null)
            {
                return new OperationResult<Usuario> { Message = "Role não encontrada", StatusCode = 404 };
            }

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password),
                RoleId = usuarioDto.RoleId,
            };

            await _usuarioRepository.AddAsync(usuario);

            return new OperationResult<Usuario> { Result = usuario };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar usuário");
            return new OperationResult<Usuario> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
