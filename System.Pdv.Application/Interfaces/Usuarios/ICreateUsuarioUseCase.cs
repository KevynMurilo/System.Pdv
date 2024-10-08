﻿using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface ICreateUsuarioUseCase
{
    Task<OperationResult<Usuario>> ExecuteAsync(CreateUsuarioDto usuario);
}
