﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Usuarios;

public class DeleteUsuarioUseCase : IDeleteUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<DeleteUsuarioUseCase> _logger;

    public DeleteUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        ILogger<DeleteUsuarioUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> ExecuteAsync(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return new OperationResult<Usuario> { Message = "Usuário não encontrado", StatusCode = 404 };

            await _usuarioRepository.DeleteAsync(usuario);
            return new OperationResult<Usuario> { Result = usuario, Message = "Usuário deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar usuário");
            return new OperationResult<Usuario> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
