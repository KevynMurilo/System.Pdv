using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface ICreateProdutoUseCase
{
    Task<OperationResult<Produto>> ExecuteAsync(CreateProdutoDto produtoDto);
}
