﻿using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface IGetAllUsuarioUseCase
{
    Task<OperationResult<IEnumerable<Usuario>>> ExecuteAsync(int pageNumber, int pageSize);
}
