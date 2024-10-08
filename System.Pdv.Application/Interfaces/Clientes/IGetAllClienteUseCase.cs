﻿using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Clientes;

public interface IGetAllClienteUseCase
{
    Task<OperationResult<IEnumerable<Cliente>>> ExecuteAsync(int pageNumber, int pageSize);
}
